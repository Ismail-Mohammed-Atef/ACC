using ACC.ViewModels.IssueCommentVM;
using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ACC.Controllers
{
    [Authorize]
    public class IssueCommentController : Controller
    {
        private readonly IIssueCommentRepository _commentRepo;
        private readonly IIssueNotificationRepository _notificationRepo;
        private readonly IIssueRepository _issueRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public IssueCommentController(
            IIssueCommentRepository commentRepo,
            IIssueNotificationRepository notificationRepo,
            IIssueRepository issueRepo,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment env)
        {
            _commentRepo = commentRepo;
            _notificationRepo = notificationRepo;
            _issueRepo = issueRepo;
            _userManager = userManager;
            _env = env;
        }

        public async Task<IActionResult> Index(int issueId)
        {
            var user = await _userManager.GetUserAsync(User);

            // ✅ تعليم الإشعارات كمقروءة باستخدام الـ Notification Repository
            var unreadNotifications = await _notificationRepo
                .GetUnreadByIssueAndUserAsync(issueId, user.Id);

            foreach (var note in unreadNotifications)
            {
                note.IsRead = true;
            }

            _notificationRepo.Save(); // أو await إذا عندك async

            // ✅ جلب الـ ProjectId من الريبو
            var issue = await _issueRepo.GetByIdAsync(issueId);
            ViewBag.ProjectId = issue?.ProjectId;
            ViewBag.IssueId = issueId;

            var comments = await _commentRepo.GetCommentsByIssueIdAsync(issueId);

            var viewModel = comments.Select(c => new IssueCommentVM
            {
                Id = c.Id,
                IssueId = c.IssueId,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                AuthorName = c.Author?.UserName ?? "User"
            }).ToList();

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(int issueId, string content, IFormFile? image)
        {
            var user = await _userManager.GetUserAsync(User);
            string? imagePath = null;

            if (image != null && image.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "comments", issueId.ToString());
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                imagePath = $"/uploads/comments/{issueId}/{fileName}";
            }

            if (string.IsNullOrWhiteSpace(content) && imagePath == null)
                return RedirectToAction("Index", new { issueId });

            var comment = new IssueComment
            {
                IssueId = issueId,
                AuthorId = user.Id,
                Content = content,
                CreatedAt = DateTime.UtcNow,
                ImagePath = imagePath
            };

            _commentRepo.Insert(comment);
            _commentRepo.Save();

            return RedirectToAction("Index", new { issueId });
        }
    }
}

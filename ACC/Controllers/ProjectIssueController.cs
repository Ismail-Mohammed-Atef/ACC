using ACC.Services;
using ACC.ViewModels;
using BusinessLogic.Repository.RepositoryClasses;
using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer;
using DataLayer.Models;
using DataLayer.Models.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ACC.Controllers
{
    public class ProjectIssueController : Controller
    {
        private readonly IIssueRepository issueRepository;
        private readonly IssueReviewersService issueReviewersService;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<ApplicationUser> userManager;

        public ProjectIssueController(IIssueRepository issueRepository, IssueReviewersService issueReviewersService, AppDbContext context, IWebHostEnvironment env, UserManager<ApplicationUser> userManager)
        {
            this.issueRepository = issueRepository;
            this.issueReviewersService = issueReviewersService;
            _context = context;
            _env = env;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(int id)
        {
            var CurrentUser = await userManager.GetUserAsync(User);
            List<Issue> issues = issueRepository.GetIssuesByUserId(CurrentUser.Id, id);

            var issueViewModels = issues.Select(i => new ProjectIssueVM
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                Category = i.Category,
                Type = i.Type,
                Priority = i.Priority,
                Status = i.Status,
                ProjectId = i.ProjectId,
                //DocumentId = i.DocumentId
                DocumentId = i.Document?.Versions.FirstOrDefault()?.Id

            }).ToList();

            ViewBag.Id = id;
            return View(issueViewModels);
        }

        public async Task<IActionResult> Create(int id) // ← id = ProjectId من URL
        {
            var currentUser = await userManager.GetUserAsync(User);

            var vm = new ProjectIssueVM
            {
                ProjectId = id, // ← نسجله تلقائيًا
                applicationUsers = userManager.Users
                    .Where(u => u.Id != currentUser.Id)
                    .ToList()
            };

            return View("Create", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectIssueVM model)
        {
            var CurrentUser = await userManager.GetUserAsync(User);

            var issue = new Issue
            {
                Title = model.Title,
                Description = model.Description,
                Category = model.Category,
                Type = model.Type,
                Priority = model.Priority,
                Status = model.Status,
                ProjectId = model.ProjectId,
                InitiatorID = CurrentUser.Id
            };

            issueRepository.Insert(issue);
            issueRepository.Save();

            foreach (var id in model.SelectedReviewerIds)
            {
                IssueReviwers issueReviwers = new IssueReviwers()
                {
                    IssueId = issue.Id,
                    ReviewerId = id
                };
                issueReviewersService.Insert(issueReviwers);
            }
            issueReviewersService.Save();

            if (model.Attachment != null && model.Attachment.Length > 0)
            {
                int projectId = model.ProjectId;
                var project = await _context.Projects.FindAsync(projectId);
                if (project == null)
                {
                    ModelState.AddModelError("ProjectId", "Project not found.");
                    return View(model);
                }

                var extension = Path.GetExtension(model.Attachment.FileName).ToLower();

                // Get or create "Work In Progress" folder
                var wipFolder = await _context.Folders
                    .FirstOrDefaultAsync(f => f.ProjectId == projectId && f.Name == "Work In Progress" && f.ParentFolderId == null);
                if (wipFolder == null)
                {
                    wipFolder = new Folder
                    {
                        Name = "Work In Progress",
                        ParentFolderId = null,
                        ProjectId = projectId,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = User.Identity.Name ?? "System"
                    };
                    _context.Folders.Add(wipFolder);
                    await _context.SaveChangesAsync();
                }

                // Get or create "Issues" subfolder
                var issuesFolder = await _context.Folders
                    .FirstOrDefaultAsync(f => f.ProjectId == projectId && f.Name == "Issues" && f.ParentFolderId == wipFolder.Id);
                if (issuesFolder == null)
                {
                    issuesFolder = new Folder
                    {
                        Name = "Issues",
                        ParentFolderId = wipFolder.Id,
                        ProjectId = projectId,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = User.Identity.Name ?? "System"
                    };
                    _context.Folders.Add(issuesFolder);
                    await _context.SaveChangesAsync();
                }

                // Create issue folder named: {IssueId}_{Title}
                var folderName = $"{issue.Id}_{CleanFileName(issue.Title)}";
                var issueFolderPath = Path.Combine(_env.WebRootPath, "uploads", projectId.ToString(), wipFolder.Id.ToString(), issuesFolder.Id.ToString(), folderName);
                Directory.CreateDirectory(issueFolderPath);

                // Upload file
                var fileName = $"{Guid.NewGuid()}_{model.Attachment.FileName}";
                var filePath = Path.Combine(issueFolderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Attachment.CopyToAsync(stream);
                }

                // Create document + version
                var document = new Document
                {
                    Name = Path.GetFileNameWithoutExtension(model.Attachment.FileName),
                    FileType = extension,
                    FolderId = issuesFolder.Id,
                    ProjectId = projectId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = User.Identity.Name ?? "System",
                    Versions = new List<DocumentVersion>()
                };

                var version = new DocumentVersion
                {
                    FilePath = filePath,
                    UploadedAt = DateTime.UtcNow,
                    UploadedBy = User.Identity.Name ?? "System",
                    VersionNumber = 1
                };

                document.Versions.Add(version);
                _context.Documents.Add(document);
                await _context.SaveChangesAsync();

                issue.DocumentId = document.Id;

                var existingIssue = issueRepository.GetById(issue.Id);
                if (existingIssue == null)
                    throw new Exception("Issue not found.");

                existingIssue.DocumentId = document.Id;
                issueRepository.Update(existingIssue);
                issueRepository.Save();

            }

            return RedirectToAction("Index", new { id = model.ProjectId });
        }

        // Helper method to sanitize folder names
        private string CleanFileName(string name)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(c, '_');
            }
            return name.Replace(" ", "_").Trim();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var issue = issueRepository.GetById(id);
            if (issue == null)
                return NotFound();

            var currentUser = await userManager.GetUserAsync(User);

            var model = new ProjectIssueVM
            {
                Id = issue.Id,
                Title = issue.Title,
                Description = issue.Description,
                Category = issue.Category,
                Type = issue.Type,
                Priority = issue.Priority,
                Status = issue.Status,
                ProjectId = issue.ProjectId,
                DocumentId = issue.DocumentId,
                applicationUsers = userManager.Users
                    .Where(u => u.Id != currentUser.Id)
                    .ToList()
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(ProjectIssueVM model)
        {
            var issue = issueRepository.GetById(model.Id);
            if (issue == null)
                return NotFound();

            var previousStatus = issue.Status;

            issue.Title = model.Title;
            issue.Description = model.Description;
            issue.Category = model.Category;
            issue.Type = model.Type;
            issue.Priority = model.Priority;
            issue.Status = model.Status;
            issue.ProjectId = model.ProjectId;
            issue.DocumentId = model.DocumentId;

            var extension = model.Attachment != null ? Path.GetExtension(model.Attachment.FileName).ToLower() : null;

            if (model.Attachment != null && model.Attachment.Length > 0)
            {
                var projectId = model.ProjectId;

                // Get or create Work In Progress > Issues
                var wipFolder = await _context.Folders
                    .FirstOrDefaultAsync(f => f.ProjectId == projectId && f.Name == "Work In Progress" && f.ParentFolderId == null);

                if (wipFolder == null)
                {
                    wipFolder = new Folder { Name = "Work In Progress", ProjectId = projectId, CreatedAt = DateTime.UtcNow, CreatedBy = User.Identity.Name ?? "System" };
                    _context.Folders.Add(wipFolder);
                    await _context.SaveChangesAsync();
                }

                var issuesFolder = await _context.Folders
                    .FirstOrDefaultAsync(f => f.ProjectId == projectId && f.Name == "Issues" && f.ParentFolderId == wipFolder.Id);

                if (issuesFolder == null)
                {
                    issuesFolder = new Folder { Name = "Issues", ProjectId = projectId, ParentFolderId = wipFolder.Id, CreatedAt = DateTime.UtcNow, CreatedBy = User.Identity.Name ?? "System" };
                    _context.Folders.Add(issuesFolder);
                    await _context.SaveChangesAsync();
                }

                // Upload to: uploads/{ProjectId}/{WIP}/{Issues}/{IssueId}_{Title}
                var folderName = $"{issue.Id}_{CleanFileName(issue.Title)}";
                var issueFolderPath = Path.Combine(_env.WebRootPath, "uploads", projectId.ToString(), wipFolder.Id.ToString(), issuesFolder.Id.ToString(), folderName);
                Directory.CreateDirectory(issueFolderPath);

                var fileName = $"{Guid.NewGuid()}_{model.Attachment.FileName}";
                var filePath = Path.Combine(issueFolderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Attachment.CopyToAsync(stream);
                }

                var document = new Document
                {
                    Name = Path.GetFileNameWithoutExtension(model.Attachment.FileName),
                    FileType = extension,
                    FolderId = issuesFolder.Id,
                    ProjectId = projectId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = User.Identity.Name ?? "System",
                    Versions = new List<DocumentVersion>()
                };

                var version = new DocumentVersion
                {
                    FilePath = filePath,
                    UploadedAt = DateTime.UtcNow,
                    UploadedBy = User.Identity.Name ?? "System",
                    VersionNumber = 1
                };

                document.Versions.Add(version);
                _context.Documents.Add(document);
                await _context.SaveChangesAsync();

                issue.DocumentId = document.Id;
            }

            // ✅ Move to Archive if status changed to Closed
            if (previousStatus != IssueStatus.Closed && model.Status == IssueStatus.Closed && issue.DocumentId != null)
            {
                var document = await _context.Documents
                    .Include(d => d.Versions)
                    .FirstOrDefaultAsync(d => d.Id == issue.DocumentId);

                var oldVersion = document?.Versions?.FirstOrDefault();
                if (oldVersion != null && System.IO.File.Exists(oldVersion.FilePath))
                {
                    // Get or create Archive > Issues
                    var archiveFolder = await _context.Folders
                        .FirstOrDefaultAsync(f => f.ProjectId == issue.ProjectId && f.Name == "Archive" && f.ParentFolderId == null);
                    if (archiveFolder == null)
                    {
                        archiveFolder = new Folder { Name = "Archive", ProjectId = issue.ProjectId, CreatedAt = DateTime.UtcNow, CreatedBy = User.Identity.Name ?? "System" };
                        _context.Folders.Add(archiveFolder);
                        await _context.SaveChangesAsync();
                    }

                    var archiveIssuesFolder = await _context.Folders
                        .FirstOrDefaultAsync(f => f.Name == "Issues" && f.ParentFolderId == archiveFolder.Id && f.ProjectId == issue.ProjectId);
                    if (archiveIssuesFolder == null)
                    {
                        archiveIssuesFolder = new Folder { Name = "Issues", ParentFolderId = archiveFolder.Id, ProjectId = issue.ProjectId, CreatedAt = DateTime.UtcNow, CreatedBy = User.Identity.Name ?? "System" };
                        _context.Folders.Add(archiveIssuesFolder);
                        await _context.SaveChangesAsync();
                    }

                    // إنشاء فولدر جديد باسم {IssueId}_{Title}
                    var archiveIssueFolder = Path.Combine(_env.WebRootPath, "uploads", issue.ProjectId.ToString(), archiveFolder.Id.ToString(), archiveIssuesFolder.Id.ToString(), $"{issue.Id}_{CleanFileName(issue.Title)}");
                    Directory.CreateDirectory(archiveIssueFolder);

                    // نقل الملف فعليًا
                    var oldPath = oldVersion.FilePath;
                    var newFilePath = Path.Combine(archiveIssueFolder, Path.GetFileName(oldPath));
                    System.IO.File.Copy(oldPath, newFilePath, true);

                    // تحديث المسار
                    oldVersion.FilePath = newFilePath;

                    // حذف الفولدر الخاص بالـ Issue فقط لو فاضي (مش WIP أو Issues)
                    var oldIssueFolder = Path.GetDirectoryName(oldPath);
                    if (Directory.Exists(oldIssueFolder) && !Directory.EnumerateFileSystemEntries(oldIssueFolder).Any())
                    {
                        Directory.Delete(oldIssueFolder, true);
                    }

                    await _context.SaveChangesAsync();
                }
            }

            issueRepository.Update(issue);
            issueRepository.Save();

            return RedirectToAction("Index", new { id = issue.ProjectId });
        }

      


        public IActionResult Delete(int id)
        {
            var issue = issueRepository.GetById(id);
            issueRepository.Delete(issue);
            return RedirectToAction("Index", new { id = issue.ProjectId });
        }

        //viewers

    }
}
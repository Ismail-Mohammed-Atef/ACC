using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer.Models;
using ACC.ViewModels;
using Microsoft.EntityFrameworkCore;
using DataLayer;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using System.Threading.Tasks;

namespace ACC.Controllers
{
    public class ProjectIssueController : Controller
    {
        private readonly IIssueService _issueService;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProjectIssueController(IIssueService issueService, AppDbContext context, IWebHostEnvironment env)
        {
            _issueService = issueService;
            _context = context;
            _env = env;
        }

        public IActionResult Index(int? projectId)
        {
            List<Issue> issues;
            if (projectId.HasValue)
            {
                issues = _issueService.GetIssuesByProjectId(projectId.Value);
            }
            else
            {
                issues = _issueService.GetAllIssues();
            }
            var issueViewModels = issues.Select(i => new ProjectIssueVM
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                Category = i.Category,
                Type = i.Type,
                Priority = i.Priority,
                Status = i.Status,
                ProjectId = i.ProjectId
            }).ToList();
            return View(issueViewModels);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectIssueVM model)
        {
            if (ModelState.IsValid)
            {
                var issue = new Issue
                {
                    Title = model.Title,
                    Description = model.Description,
                    Category = model.Category,
                    Type = model.Type,
                    Priority = model.Priority,
                    Status = model.Status,
                    ProjectId = model.ProjectId
                };
                _issueService.CreateIssue(issue);

                // التعامل مع الـ Attachment
                if (model.Attachment != null && model.Attachment.Length > 0)
                {
                    // تحديد الـ ProjectId
                    int projectId = model.ProjectId;

                    // البحث عن Root Folder باسم "ProjectRoot"
                    var rootFolder = await _context.Folders
                        .FirstOrDefaultAsync(f => f.ProjectId == projectId && f.ParentFolderId == null && f.Name == "ProjectRoot");

                    if (rootFolder == null)
                    {
                        // لو مش موجود، اعمل Root Folder جديد
                        rootFolder = new Folder
                        {
                            Name = "ProjectRoot",
                            ParentFolderId = null,
                            ProjectId = projectId,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = User.Identity.Name ?? "System"
                        };
                        _context.Folders.Add(rootFolder);
                        await _context.SaveChangesAsync();
                    }

                    // البحث عن SubFolder باسم "Issues"
                    var issuesFolder = await _context.Folders
                        .FirstOrDefaultAsync(f => f.ParentFolderId == rootFolder.Id && f.Name == "Issues");

                    if (issuesFolder == null)
                    {
                        // لو مش موجود، اعمل SubFolder جديد باسم "Issues"
                        issuesFolder = new Folder
                        {
                            Name = "Issues",
                            ParentFolderId = rootFolder.Id,
                            ProjectId = projectId,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = User.Identity.Name ?? "System"
                        };
                        _context.Folders.Add(issuesFolder);
                        await _context.SaveChangesAsync();
                    }

                    // رفع الـ Attachment وإضافته كـ Document
                    var allowedExtensions = new[] { ".pdf", ".docx", ".jpg", ".png" };
                    var extension = Path.GetExtension(model.Attachment.FileName).ToLower();
                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("Attachment", "Invalid file type. Allowed types: PDF, DOCX, JPG, PNG.");
                        return View(model);
                    }

                    var uploadFolder = Path.Combine(_env.WebRootPath, "uploads", projectId.ToString(), issuesFolder.Id.ToString());
                    Directory.CreateDirectory(uploadFolder);

                    var fileName = $"{Guid.NewGuid()}_{model.Attachment.FileName}";
                    var filePath = Path.Combine(uploadFolder, fileName);

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
                }

                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var issue = _issueService.GetIssueById(id);
            var model = new ProjectIssueVM
            {
                Id = issue.Id,
                Title = issue.Title,
                Description = issue.Description,
                Category = issue.Category,
                Type = issue.Type,
                Priority = issue.Priority,
                Status = issue.Status,
                ProjectId = issue.ProjectId
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(ProjectIssueVM model)
        {
            if (ModelState.IsValid)
            {
                var issue = new Issue
                {
                    Id = model.Id,
                    Title = model.Title,
                    Description = model.Description,
                    Category = model.Category,
                    Type = model.Type,
                    Priority = model.Priority,
                    Status = model.Status,
                    ProjectId = model.ProjectId
                };
                _issueService.UpdateIssue(issue);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            _issueService.DeleteIssue(id);
            return RedirectToAction("Index");
        }
    }
}
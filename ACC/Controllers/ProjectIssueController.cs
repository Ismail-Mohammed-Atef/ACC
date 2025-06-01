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
                ProjectId = i.ProjectId,
                DocumentId = i.DocumentId
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

                if (model.Attachment != null && model.Attachment.Length > 0)
                {
                    int projectId = model.ProjectId;
                    var project = await _context.Projects.FindAsync(projectId);
                    if (project == null)
                    {
                        ModelState.AddModelError("ProjectId", "Project not found.");
                        return View(model);
                    }
                    string projectFolderName = project.Name;

                    var projectFolder = await _context.Folders
                        .FirstOrDefaultAsync(f => f.ProjectId == projectId && f.ParentFolderId == null && f.Name == projectFolderName);

                    if (projectFolder == null)
                    {
                        projectFolder = new Folder
                        {
                            Name = projectFolderName,
                            ParentFolderId = null,
                            ProjectId = projectId,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = User.Identity.Name ?? "System"
                        };
                        _context.Folders.Add(projectFolder);
                        await _context.SaveChangesAsync();
                    }

                    var issuesFolder = await _context.Folders
                        .FirstOrDefaultAsync(f => f.ParentFolderId == projectFolder.Id && f.Name == "Issues");

                    if (issuesFolder == null)
                    {
                        issuesFolder = new Folder
                        {
                            Name = "Issues",
                            ParentFolderId = projectFolder.Id,
                            ProjectId = projectId,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = User.Identity.Name ?? "System"
                        };
                        _context.Folders.Add(issuesFolder);
                        await _context.SaveChangesAsync();
                    }

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

                    issue.DocumentId = document.Id;
                    _issueService.UpdateIssue(issue);
                }

                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var issue = _issueService.GetIssueById(id);
            if (issue == null)
            {
                return NotFound();
            }

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
                DocumentId = issue.DocumentId // جلب الـ DocumentId الحالي
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProjectIssueVM model)
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
                    ProjectId = model.ProjectId,
                    DocumentId = model.DocumentId // الـ DocumentId الحالي
                };

                // التعامل مع الـ Attachment الجديد
                if (model.Attachment != null && model.Attachment.Length > 0)
                {
                    int projectId = model.ProjectId;
                    var project = await _context.Projects.FindAsync(projectId);
                    if (project == null)
                    {
                        ModelState.AddModelError("ProjectId", "Project not found.");
                        return View(model);
                    }
                    string projectFolderName = project.Name;

                    var projectFolder = await _context.Folders
                        .FirstOrDefaultAsync(f => f.ProjectId == projectId && f.ParentFolderId == null && f.Name == projectFolderName);

                    if (projectFolder == null)
                    {
                        projectFolder = new Folder
                        {
                            Name = projectFolderName,
                            ParentFolderId = null,
                            ProjectId = projectId,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = User.Identity.Name ?? "System"
                        };
                        _context.Folders.Add(projectFolder);
                        await _context.SaveChangesAsync();
                    }

                    var issuesFolder = await _context.Folders
                        .FirstOrDefaultAsync(f => f.ParentFolderId == projectFolder.Id && f.Name == "Issues");

                    if (issuesFolder == null)
                    {
                        issuesFolder = new Folder
                        {
                            Name = "Issues",
                            ParentFolderId = projectFolder.Id,
                            ProjectId = projectId,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = User.Identity.Name ?? "System"
                        };
                        _context.Folders.Add(issuesFolder);
                        await _context.SaveChangesAsync();
                    }

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

                    issue.DocumentId = document.Id; 
                }

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
using ACC.Services;
using ACC.ViewModels;
using BusinessLogic.Repository.RepositoryClasses;
using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer;
using DataLayer.Models;
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
                DocumentId = i.DocumentId
            }).ToList();

            ViewBag.Id = id;
            return View(issueViewModels);
        }

        public IActionResult Create()
        {
            ProjectIssueVM vm = new ProjectIssueVM();
            vm.applicationUsers = userManager.Users.ToList();
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
                InitiatorID = CurrentUser.Id,

            };
            issueRepository.Insert(issue);
            issueRepository.Save();




            foreach (var id in model.SelectedReviewerIds)
            {
                IssueReviwers issueReviwers = new IssueReviwers()
                {
                    IssueId = issue.Id,
                    ReviewerId = id,
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

                var existingIssue = issueRepository.GetById(issue.Id);
                if (existingIssue == null)
                    throw new Exception("Issue not found.");


                existingIssue.Title = issue.Title;
                existingIssue.Description = issue.Description;
                existingIssue.Category = issue.Category;
                existingIssue.Type = issue.Type;
                existingIssue.Priority = issue.Priority;
                existingIssue.Status = issue.Status;
                existingIssue.ProjectId = issue.ProjectId;
                existingIssue.DocumentId = issue.DocumentId;

                issueRepository.Update(existingIssue);



            }

            return RedirectToAction("Index", new { id = model.ProjectId });


        }

        public IActionResult Edit(int id)
        {
            var issue = issueRepository.GetById(id);
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
                DocumentId = issue.DocumentId
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProjectIssueVM model)
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

            var existingIssue = issueRepository.GetById(issue.Id);
            if (existingIssue == null)
                throw new Exception("Issue not found.");


            existingIssue.Title = issue.Title;
            existingIssue.Description = issue.Description;
            existingIssue.Category = issue.Category;
            existingIssue.Type = issue.Type;
            existingIssue.Priority = issue.Priority;
            existingIssue.Status = issue.Status;
            existingIssue.ProjectId = issue.ProjectId;
            existingIssue.DocumentId = issue.DocumentId;

            issueRepository.Update(existingIssue);
            issueRepository.Save();
            return RedirectToAction("Index", new { id = issue.ProjectId });

            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var issue = issueRepository.GetById(id);
            issueRepository.Delete(issue);
            return RedirectToAction("Index", new { id = issue.ProjectId });
        }
    }
}
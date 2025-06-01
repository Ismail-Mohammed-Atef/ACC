using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ACC.Controllers.ProjectDetailsController
{
    public class ProjectDocumentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProjectDocumentController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _env = env ?? throw new ArgumentNullException(nameof(env));
        }

        [HttpGet]
        public async Task<IActionResult> Index(int Id)
        {
            try
            {
                // Check if project exists
                var project = await _context.Projects.FindAsync(Id);
                if (project == null)
                {
                    return NotFound("Project not found.");
                }

                // Get all folders for the project
                var folders = await _context.Folders
                    .Where(f => f.ProjectId == Id)
                    .Include(f => f.SubFolders)
                    .Include(f => f.Documents)
                    .ToListAsync();

                ViewBag.ProjectId = Id;

                if (!folders.Any())
                {
                    // Return an empty folder model to indicate no folders exist
                    var emptyFolder = new Folder
                    {
                        Name = "No Folders",
                        ProjectId = Id,
                        SubFolders = new List<Folder>(),
                        Documents = new List<Document>()
                    };
                    return View(new List<Folder> { emptyFolder });
                }

                return View(folders);
            }
            catch (Exception ex)
            {
                // Log the exception (use a logging framework like Serilog)
                TempData["Error"] = "An error occurred while loading folders.";
                return RedirectToAction("Index", new { Id });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Folder(int? id, int projectId)
        {
            try
            {
                Folder? folder;

                if (id == null)
                {
                    // Check if a root folder exists for the project
                    folder = await _context.Folders
                        .FirstOrDefaultAsync(f => f.ProjectId == projectId && f.ParentFolderId == null);

                    if (folder == null)
                    {
                        return RedirectToAction("CreateRoot", new { projectId });
                    }
                }
                else
                {
                    folder = await _context.Folders
                        .Include(f => f.SubFolders)
                        .Include(f => f.Documents)
                        .ThenInclude(d => d.Versions)
                        .FirstOrDefaultAsync(f => f.Id == id && f.ProjectId == projectId);
                }

                if (folder == null)
                {
                    return NotFound("Folder not found or you don't have access.");
                }

                ViewBag.ProjectId = projectId;
                return View(folder);
            }
            catch (Exception ex)
            {
                // Log the exception
                TempData["Error"] = "An error occurred while loading the folder.";
                return RedirectToAction("Index", new { projectId });
            }
        }

        [HttpGet]
        public IActionResult CreateRoot(int projectId)
        {
            ViewBag.ProjectId = projectId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRoot(int projectId, string folderName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(folderName))
                {
                    ModelState.AddModelError("FolderName", "Folder name is required.");
                    ViewBag.ProjectId = projectId;
                    return View();
                }

                var project = await _context.Projects.FindAsync(projectId);
                if (project == null)
                {
                    return NotFound("Project not found.");
                }

                if (await _context.Folders.AnyAsync(f => f.ProjectId == projectId && f.ParentFolderId == null))
                {
                    TempData["Error"] = "A root folder already exists for this project.";
                    return RedirectToAction("Index", new { projectId });
                }

                var root = new Folder
                {
                    Name = folderName,
                    ParentFolderId = null,
                    ProjectId = projectId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = User.Identity.Name ?? "System"
                };

                _context.Folders.Add(root);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Root folder created successfully.";
                return RedirectToAction("Folder", new { id = root.Id, projectId });
            }
            catch (Exception ex)
            {
                // Log the exception
                TempData["Error"] = "An error occurred while creating the root folder.";
                ViewBag.ProjectId = projectId;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFolder(int parentFolderId, int projectId, string folderName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(folderName))
                {
                    TempData["Error"] = "Folder name is required.";
                    return RedirectToAction("Folder", new { id = parentFolderId, projectId });
                }

                var parentFolder = await _context.Folders
                    .FirstOrDefaultAsync(f => f.Id == parentFolderId && f.ProjectId == projectId);

                if (parentFolder == null)
                {
                    return NotFound("Parent folder not found.");
                }

                var folder = new Folder
                {
                    Name = folderName,
                    ParentFolderId = parentFolderId,
                    ProjectId = projectId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = User.Identity.Name ?? "System"
                };

                _context.Folders.Add(folder);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Folder created successfully.";
                return RedirectToAction("Folder", new { id = parentFolderId, projectId });
            }
            catch (Exception ex)
            {
                // Log the exception
                TempData["Error"] = "An error occurred while creating the folder.";
                return RedirectToAction("Folder", new { id = parentFolderId, projectId });
            }
        }

        [HttpGet]
        public IActionResult Upload(int folderId, int projectId)
        {
            ViewBag.FolderId = folderId;
            ViewBag.ProjectId = projectId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(int folderId, int projectId, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    TempData["Error"] = "No file selected.";
                    ViewBag.FolderId = folderId;
                    ViewBag.ProjectId = projectId;
                    return View();
                }

                if (file.Length > 10 * 1024 * 1024)
                {
                    TempData["Error"] = "File size exceeds 10MB limit.";
                    ViewBag.FolderId = folderId;
                    ViewBag.ProjectId = projectId;
                    return View();
                }

                var allowedExtensions = new[] { ".pdf", ".docx", ".jpg", ".png" };
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    TempData["Error"] = "Invalid file type. Allowed types: PDF, DOCX, JPG, PNG.";
                    ViewBag.FolderId = folderId;
                    ViewBag.ProjectId = projectId;
                    return View();
                }

                var folder = await _context.Folders
                    .FirstOrDefaultAsync(f => f.Id == folderId && f.ProjectId == projectId);

                if (folder == null)
                {
                    return NotFound("Folder not found.");
                }

                var uploadFolder = Path.Combine(_env.WebRootPath, "uploads", projectId.ToString(), folderId.ToString());
                Directory.CreateDirectory(uploadFolder);

                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var document = await _context.Documents
                    .Include(d => d.Versions)
                    .FirstOrDefaultAsync(d => d.FolderId == folderId && d.Name == Path.GetFileNameWithoutExtension(file.FileName));

                if (document == null)
                {
                    document = new Document
                    {
                        Name = Path.GetFileNameWithoutExtension(file.FileName),
                        FileType = extension,
                        FolderId = folderId,
                        ProjectId = projectId,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = User.Identity.Name ?? "System",
                        Versions = new List<DocumentVersion>()
                    };
                    _context.Documents.Add(document);
                }

                var version = new DocumentVersion
                {
                    FilePath = filePath,
                    UploadedAt = DateTime.UtcNow,
                    UploadedBy = User.Identity.Name ?? "System",
                    VersionNumber = document.Versions.Count + 1
                };

                document.Versions.Add(version);
                await _context.SaveChangesAsync();

                TempData["Success"] = "File uploaded successfully.";
                return RedirectToAction("Folder", new { id = folderId, projectId });
            }
            catch (Exception ex)
            {
                // Log the exception
                TempData["Error"] = "An error occurred while uploading the file.";
                ViewBag.FolderId = folderId;
                ViewBag.ProjectId = projectId;
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Download(int versionId, int projectId)
        {
            try
            {
                var version = await _context.DocumentVersions
                    .Include(v => v.Document)
                    .FirstOrDefaultAsync(v => v.Id == versionId && v.Document.ProjectId == projectId);

                if (version == null || !System.IO.File.Exists(version.FilePath))
                {
                    return NotFound("File not found.");
                }

                var fileName = Path.GetFileName(version.FilePath);
                var fileBytes = await System.IO.File.ReadAllBytesAsync(version.FilePath);
                return File(fileBytes, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                // Log the exception
                TempData["Error"] = "An error occurred while downloading the file.";
                return RedirectToAction("Index", new { projectId });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Versions(int documentId, int projectId)
        {
            try
            {
                var document = await _context.Documents
                    .Include(d => d.Versions)
                    .FirstOrDefaultAsync(d => d.Id == documentId && d.ProjectId == projectId);

                if (document == null)
                {
                    return NotFound("Document not found.");
                }

                ViewBag.ProjectId = projectId;
                return View(document);
            }
            catch (Exception ex)
            {
                // Log the exception
                TempData["Error"] = "An error occurred while loading document versions.";
                return RedirectToAction("Index", new { projectId });
            }
        }
    }
}
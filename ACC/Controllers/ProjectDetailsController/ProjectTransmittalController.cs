using ACC.ViewModels.TransmittalsVM;
using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer.Models;
using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ACC.Controllers.ProjectDetailsController
{
    public class ProjectTransmittalController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ITransmittalRepository _transmittalRepository;

        public ProjectTransmittalController(AppDbContext context, ITransmittalRepository transmittalRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _transmittalRepository = transmittalRepository ?? throw new ArgumentNullException(nameof(transmittalRepository));
        }

        [HttpGet]
        public IActionResult Index(int Id)
        {
            try
            {
                var project = _context.Projects.Find(Id);
                if (project == null)
                {
                    return NotFound("Project not found.");
                }

                var transmittals = _transmittalRepository.GetTransmittalsByProjectId(Id);
                ViewBag.ProjectId = Id;
                return View(transmittals.ToList());
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while loading transmittals.";
                return RedirectToAction("Index", new { Id });
            }
        }

        [HttpGet]
        public IActionResult Create(int projectId)
        {
            var vm = new TransmittalVM
            {
                ProjectId = projectId,
                AvailableDocumentVersions = _context.DocumentVersions
                    .Include(dv => dv.Document)
                    .Where(dv => dv.Document.ProjectId == projectId)
                    .ToList()
            };
            ViewBag.ProjectId = projectId;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TransmittalVM vm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(vm.Title) || string.IsNullOrWhiteSpace(vm.Recipient))
                {
                    TempData["Error"] = "Title and recipient are required.";
                    vm.AvailableDocumentVersions = _context.DocumentVersions
                        .Include(dv => dv.Document)
                        .Where(dv => dv.Document.ProjectId == vm.ProjectId)
                        .ToList();
                    ViewBag.ProjectId = vm.ProjectId;
                    return View(vm);
                }

                var transmittal = new Transmittal
                {
                    ProjectId = vm.ProjectId,
                    Title = vm.Title,
                    Recipient = vm.Recipient,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = User.Identity.Name ?? "System",
                    TransmittalDocuments = new List<TransmittalDocument>()
                };

                _context.Transmittals.Add(transmittal);
                _context.SaveChanges();

                if (vm.DocumentVersionIds != null && vm.Notes != null && vm.DocumentVersionIds.Count == vm.Notes.Count)
                {
                    for (int i = 0; i < vm.DocumentVersionIds.Count; i++)
                    {
                        _transmittalRepository.AddDocumentToTransmittal(transmittal.Id, vm.DocumentVersionIds[i], vm.Notes[i]);
                    }
                }

                TempData["Success"] = "Transmittal created successfully.";
                return RedirectToAction("Index", new { Id = vm.ProjectId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while creating the transmittal.";
                vm.AvailableDocumentVersions = _context.DocumentVersions
                    .Include(dv => dv.Document)
                    .Where(dv => dv.Document.ProjectId == vm.ProjectId)
                    .ToList();
                ViewBag.ProjectId = vm.ProjectId;
                return View(vm);
            }
        }

        [HttpGet]
        public IActionResult Details(int id, int projectId)
        {
            try
            {
                var transmittal = _context.Transmittals
                    .Include(t => t.TransmittalDocuments)
                    .ThenInclude(td => td.DocumentVersion)
                    .ThenInclude(dv => dv.Document)
                    .FirstOrDefault(t => t.Id == id && t.ProjectId == projectId);

                if (transmittal == null)
                {
                    return NotFound("Transmittal not found.");
                }

                ViewBag.ProjectId = projectId;
                return View(transmittal);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while loading transmittal details.";
                return RedirectToAction("Index", new { Id = projectId });
            }
        }
    }
}

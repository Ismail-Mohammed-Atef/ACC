using ACC.ViewModels.ProjectVMs;
using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer;
using DataLayer.Models;
using DataLayer.Models.Enums;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ACC.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjetcRepository projectRepo;
        private readonly IProjectActivityRepository projectActivityRepo;
        private readonly AppDbContext _context;
        public ProjectController(AppDbContext context, IProjetcRepository ProjectRepo ,IProjectActivityRepository projectActivityRepo)

        {
            _context = context;
            projectRepo = ProjectRepo;
            this.projectActivityRepo = projectActivityRepo;
        }
     

        #region Index DisplayData Action
        public IActionResult Index(string srchText, int Page = 1, int Pagesize = 5, bool showArchived = false)
        {
            var Currencies = new SelectList(Enum.GetValues(typeof(Currency)).Cast<Currency>());

            var ProjectTypes = Enum.GetValues(typeof(ProjectType)).Cast<ProjectType>()
                .Select(pt => new
                {
                    Value = pt.ToString(),
                    DisplayName = Enum_Helper.GetDescription(pt)
                })
                .ToList();

            ViewBag.Currencies = Currencies;
            ViewBag.ProjectTypes = new SelectList(ProjectTypes, "Value", "DisplayName");

            // Get paginated projects with showArchived filter
            var projects = projectRepo.GetPaginatedProjects(Page, Pagesize, srchText, showArchived);

            // If no projects are found, return an empty list
            if (projects == null || !projects.Any())
            {
                return View(new List<DisplayProjectsVM>());
            }

            // Map the projects to the ViewModel
            List<DisplayProjectsVM> displayProject = projects.Select(p => new DisplayProjectsVM
            {
                id = p.Id,
                Name = p.Name,
                ProjectNumber = p.ProjectNumber,
                ProjectType = p.ProjectType,
                ProjectValue = p.ProjectValue,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                CreationDate = p.CreationDate
            }).ToList();

            // Calculate pagination details
            int ProjectCount = projectRepo.GetProjectsCount(srchText, showArchived);
            int TotalPages = (int)Math.Ceiling((double)ProjectCount / Pagesize);

            // Pass pagination details to the view
            ViewBag.CurrentPage = Page;
            ViewBag.totalPages = TotalPages;
            ViewBag.ShowArchived = showArchived;

            return View("Index", displayProject);
        }

        #endregion



        #region Create Project Action 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProject(AddProjectVM projectFromRequest)
        {
            if (ModelState.IsValid)
            {
                Project newProject = new Project
                {

                    Name = projectFromRequest.Name,
                    ProjectNumber = projectFromRequest.ProjectNumber,
                    ProjectType = projectFromRequest.ProjectType,
                    ProjectValue = projectFromRequest.ProjectValue,
                    StartDate = projectFromRequest.StartDate,
                    EndDate = projectFromRequest.EndDate,
                    CreationDate = DateTime.Now,
                    Currency = projectFromRequest.Currency,
                    Address = projectFromRequest.Address,
                };

                projectRepo.Insert(newProject);
                projectRepo.Save();
                projectActivityRepo.AddNewActivity(newProject);

                return Json(new { success = true });
            }

            // If ModelState is invalid, return validation errors as JSON
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return Json(new { success = false, errors = errors });
        }

        #endregion


        [HttpPost]
        public IActionResult Delete(int id)
        {
            var deletedProject = projectRepo.GetById(id);
            if (deletedProject == null)
            {
                return NotFound(); // Return 404 if project isn't found
            }

            projectRepo.Delete(deletedProject);
            projectRepo.Save();
            projectActivityRepo.RemoveActivity(deletedProject);

            return Json(new { success = true });  // Return JSON response for AJAX
        }

        //[HttpPost]
        public IActionResult Archive(int id)
        {
            var project = projectRepo.GetById(id);
            if (project == null)
            {
                return NotFound();
            }
            project.IsArchived = true;
            projectRepo.Save();
            return Ok();
        }

        public IActionResult Restore(int id)
        {
            var project = projectRepo.GetById(id);
            if (project == null)
            {
                return NotFound();
            }
            project.IsArchived = false;
            projectRepo.Save();
            return Ok();
        }

    }
}

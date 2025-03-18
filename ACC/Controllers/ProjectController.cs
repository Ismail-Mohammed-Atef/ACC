using ACC.ViewModels.MemberVM.MemberVM;
using ACC.ViewModels.ProjectVMs;
using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer.Models;
using DataLayer.Models.Enums;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

namespace ACC.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjetcRepository projectRepo;

        public ProjectController(IProjetcRepository ProjectRepo)
        {
            projectRepo = ProjectRepo;
        }
        //h


        #region Index DisplayData Action
        public IActionResult Index(string srchText, int Page = 1, int Pagesize = 3)
        {
            // Get all Currency enum values
            var Currencies =new SelectList( Enum.GetValues(typeof(Currency)).Cast<Currency>());

            // Get all ProjectType enum values and format them using EnumHelper
            var ProjectTypes = Enum.GetValues(typeof(ProjectType)).Cast<ProjectType>()
                .Select(pt => new
                {
                    Value = pt.ToString(), // Use the enum value as the value
                    DisplayName = EnumHelper.GetDescription(pt) // Use the formatted display name
                })
                .ToList();

            ViewBag.Currencies = Currencies;
            ViewBag.ProjectTypes = new SelectList(ProjectTypes, "Value", "DisplayName");

            // Get paginated projects
            var projects = projectRepo.GetPaginatedProjects(Page, Pagesize, srchText);

            // If no projects are found, return an empty list
            if (projects == null || !projects.Any())
            {
                return View(new List<DisplayProjectsVM>());
            }

            // Map the projects to the ViewModel
            List<DisplayProjectsVM> displayProject = projects.Select(p => new DisplayProjectsVM
            {
                Name = p.Name,
                ProjectNumber = p.ProjectNumber,
                ProjectType = p.ProjectType,
                ProjectValue = p.ProjectValue,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                CreationDate = p.CreationDate
            }).ToList();

            // Calculate pagination details
            int ProjectCount = projectRepo.ProjectsCount();
            int TotalPages = (int)Math.Ceiling((double)ProjectCount / Pagesize);

            // Pass pagination details to the view
            ViewBag.CurrentPage = Page;
            ViewBag.totalPages = TotalPages;

            // Return the view with the paginated and filtered projects
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


        #region Search Action
        public IActionResult Search(string seachText)
        {
            if (!seachText.IsNullOrEmpty())
            {

            }

            return View("Index");
        } 
        #endregion


    }
}

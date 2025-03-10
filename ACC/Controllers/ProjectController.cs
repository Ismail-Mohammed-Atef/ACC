using ACC.ViewModels.MemberVM.MemberVM;
using ACC.ViewModels.ProjectVMs;
using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer.Models;
using DataLayer.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ACC.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjetcRepository projectRepo;

        public ProjectController(IProjetcRepository ProjectRepo)
        {
            projectRepo = ProjectRepo;
        }


        public IActionResult Index()
        {
            var projects = projectRepo.GetAll();
            if (projects == null || !projects.Any())
            {
                return View(new List<DisplayProjectsVM>());
            }

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

            return View(displayProject);
        }


        [HttpGet]
        public IActionResult AddProject()
        {
            //var currencyValues = projectRepo.GetCurrencyValues();
            //var projectTypeValues = projectRepo.GetProjectTypeValues();

            AddProjectVM model = new AddProjectVM
            {
                Currencies =   new SelectList(Enum.GetValues(typeof(Currency)).Cast<Currency>()),
                ProjectTypes = new SelectList(Enum.GetValues(typeof(ProjectType)).Cast<ProjectType>())
            };

            return PartialView("PartialViews/_AddProjectPartialView", model);
        }

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

            projectFromRequest.Currencies = new SelectList(Enum.GetValues(typeof(Currency)).Cast<Currency>());
            projectFromRequest.ProjectTypes = new SelectList(Enum.GetValues(typeof(ProjectType)).Cast<ProjectType>());
            return PartialView("PartialViews/_AddProjectPartialView", projectFromRequest);
        }


    }
}

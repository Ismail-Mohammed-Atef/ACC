
using ACC.ViewModels.ProjectVMs;
using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer.Models;
using DataLayer.Models.Enums;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace ACC.Controllers.ProjectDetailsController
{
    public class ProjectSettingController : Controller
    {

        private readonly IProjetcRepository projectRepo;

        public ProjectSettingController(IProjetcRepository projectRepo)
        {
            this.projectRepo = projectRepo;
        }


        public IActionResult Index(int id)
        {
            ViewBag.Id = id;
            var Currencies = new SelectList(Enum.GetValues(typeof(Currency)).Cast<Currency>());
            var ProjectTypes = Enum.GetValues(typeof(ProjectType)).Cast<ProjectType>()
                .Select(pt => new
                {
                    Value = pt.ToString(),
                    DisplayName = Enum_Helper.GetDescription(pt)
                })
                .ToList();
            // Project types and currencies for edit 
            ViewBag.Currencies = Currencies;
            ViewBag.ProjectTypes = new SelectList(ProjectTypes, "Value", "DisplayName");


            Project ProjectSelected = projectRepo.GetById(id);

            DisplayProjectsVM ProjectSelectedvm = new DisplayProjectsVM()
            {
                id = ProjectSelected.Id,
                Name = ProjectSelected.Name,
                ProjectNumber = ProjectSelected.ProjectNumber,
                ProjectType = ProjectSelected.ProjectType,
                ProjectValue = ProjectSelected.ProjectValue,
                StartDate = ProjectSelected.StartDate,
                EndDate = ProjectSelected.EndDate,
                Address = ProjectSelected.Address,
                Currency = ProjectSelected.Currency,
                Latitude = ProjectSelected.Latitude,
                Longitude = ProjectSelected.Longitude,
                
            };

            return View("Index", ProjectSelectedvm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProject(DisplayProjectsVM projectFromRequest)
        {

            if (!ModelState.IsValid)
            {
                var Currencies = new SelectList(Enum.GetValues(typeof(Currency)).Cast<Currency>());
                var ProjectTypes = Enum.GetValues(typeof(ProjectType)).Cast<ProjectType>()
                    .Select(pt => new
                    {
                        Value = pt.ToString(),
                        DisplayName = Enum_Helper.GetDescription(pt)
                    })
                    .ToList();
                // Project types and currencies for edit 
                ViewBag.Currencies = Currencies;
                ViewBag.ProjectTypes = new SelectList(ProjectTypes, "Value", "DisplayName");

            }
            try
            {
                var project = projectRepo.GetById(projectFromRequest.id);
                if (project == null)
                    return NotFound();

                // Update project properties
                project.Name = projectFromRequest.Name;
                project.Address = projectFromRequest.Address;
                project.StartDate = projectFromRequest.StartDate;
                project.EndDate = projectFromRequest.EndDate;
                project.ProjectNumber = projectFromRequest.ProjectNumber;
                project.ProjectType = projectFromRequest.ProjectType;
                project.ProjectValue = projectFromRequest.ProjectValue;
                project.Currency = projectFromRequest.Currency;

                projectRepo.Update(project);
                projectRepo.Save();

                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new { success = false, error = "An error occurred while updating the project" });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateProjectLocation(DisplayProjectsVM project)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, error = "Invalid input." });

            try
            {
                var existing = projectRepo.GetById(project.id);
                if (existing == null)
                    return NotFound();

                // Only update location-related fields
                existing.Address = project.Address;
                existing.Latitude = project.Latitude;
                existing.Longitude = project.Longitude;

                projectRepo.Update(existing);
                projectRepo.Save();
                ViewBag.Id = existing.Id;

                return Json(new { success = true, newAddress = project.Address });
            }
            catch
            {
                return Json(new { success = false, error = "Failed to update location." });
            }
        }

    }
}

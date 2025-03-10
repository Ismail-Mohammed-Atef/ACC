using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace ACC.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjetcRepository projectRepo;

        public ProjectController (IProjetcRepository ProjectRepo)
        {
            projectRepo = ProjectRepo;
        }


        public IActionResult Index()
        {
            var projects = projectRepo.GetAll();

            return View(projects);
        }

    }
}

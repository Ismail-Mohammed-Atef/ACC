using BusinessLogic.Repository.RepositoryInterfaces;
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

            return View();
        }




    }
}

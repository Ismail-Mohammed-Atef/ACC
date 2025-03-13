using ACC.ViewModels.RoleVM;
using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;


namespace ACC.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleRepository _roleRepository;

        public RoleController(IRoleRepository roleRepository)
        {
            this._roleRepository = roleRepository;
        }


        public IActionResult Index()
        {
            var Roles = _roleRepository.GetAll();


            var roleViewModel = Roles.Select(x => new RoleViewModel
            {

                RoleName = x.Name
            }).ToList();
            return View(roleViewModel);



        }


        [HttpPost]
        public IActionResult AddRole(string roleName)
        {
            if (!string.IsNullOrWhiteSpace(roleName))
            {
                var existingRole = _roleRepository.GetAll().FirstOrDefault(r => r.Name == roleName);
                if (existingRole == null)
                {
                    var newRole = new Role { Name = roleName };
                    _roleRepository.Insert(newRole);
                    _roleRepository.Save();

                    return Json(new { success = true, message = "Role added successfully", role = newRole.Name });
                }
                return Json(new { success = false, message = "Role already exists" });
            }
            return Json(new { success = false, message = "Invalid role name" });
        }

    }
}

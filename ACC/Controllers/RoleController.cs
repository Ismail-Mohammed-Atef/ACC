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


        // helooo gaber
        // helooo gaber



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


        [HttpPost]
        public IActionResult DeleteRole(string roleName)
        {
            if (!string.IsNullOrWhiteSpace(roleName))
            {
                var roleToDelete = _roleRepository.GetAll().FirstOrDefault(r => r.Name == roleName);
                if (roleToDelete != null)
                {
                    _roleRepository.Delete(roleToDelete);
                    _roleRepository.Save();

                    return Json(new { success = true, message = "Role deleted successfully" });
                }
                return Json(new { success = false, message = "Role not found" });
            }
            return Json(new { success = false, message = "Invalid role name" });
        }


    }
}

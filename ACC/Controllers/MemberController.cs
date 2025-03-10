
using ACC.ViewModels;
using DataLayer.Models;
using DataLayer.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ACC.Controllers
{
    public class MemberController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public MemberController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var Users = _userManager.Users.ToList();
            List<MemberVM> members = new List<MemberVM>();
            for (int i = 0; i < Users.Count; i++)
            {
                members.Add(new MemberVM()
                {
                    Id = Users[i].Id,
                    Name = Users[i].UserName,
                    Email = Users[i].Email,
                    Role = Users[i].Role?.Name ?? "No Current Roles",
                    Company = Users[i].Company?.Name ?? "No Current Company",
                    Status = Users[i].Status,
                    AddedOn = Users[i].AddedOn,
                    AccessLevels = new List<AccessLevel>()
                });
                foreach(var accessLevel in Users[i].AccessLevel){
                    members[i].AccessLevels.Add(accessLevel);
                 }
            }
            return View(members);
        }
        [HttpPost]
        public async Task<IActionResult> InsertMemberAsync(InsertMemberVM memberFromReq)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser User = new ApplicationUser()
                {
                    UserName = memberFromReq.Email.Split("@")[0],
                    Email = memberFromReq.Email,
                    RoleId = memberFromReq.RoleId,
                    CompanyId = memberFromReq.CompanyId,
                    Status = memberFromReq.Status,
                    AccessLevel = new List<AccessLevel>()
                };
                if (memberFromReq.adminAccess)
                {
                    User.AccessLevel.Add(AccessLevel.AccountAdmin);
                }
                if (memberFromReq.excutive)
                {
                    User.AccessLevel.Add(AccessLevel.Excutive);
                }
                if(memberFromReq.standardAccess)
                {
                    User.AccessLevel.Add(AccessLevel.StandardAccess);
                }
                var result = await _userManager.CreateAsync(User,"123Aa_");
                if (result.Succeeded)
                {
                return Json(new { success = true }); // Indicate success
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return PartialView("PartialViews/_AddMemberPartialView", memberFromReq);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var member = _userManager.Users.FirstOrDefault(u => u.Id == id);
            if (member != null)
            {
                var result = await _userManager.DeleteAsync(member);
                if (result.Succeeded)
                {
                return Ok();
                }
            }
            return NotFound();
        }
    }
}

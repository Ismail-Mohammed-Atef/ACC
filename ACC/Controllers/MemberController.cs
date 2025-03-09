
using ACC.ViewModels;
using DataLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
            return View(Users);
        }
        //public IActionResult InsertMember(InsertMemberVM memberFromReq)
        //{
        //    if (memberFromReq == null)
        //    {
        //        ApplicationUser User = new ApplicationUser()
        //        {
        //            UserName = memberFromReq.Name,
        //            Email = memberFromReq.Email,
        //            RoleId = memberFromReq.RoleId,
        //            CompanyId = memberFromReq.CompanyId,
        //        };
        //    }
        //}
    }
}

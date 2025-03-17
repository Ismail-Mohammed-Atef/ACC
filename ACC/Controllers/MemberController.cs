using ACC.ViewModels.MemberVM;
using ACC.ViewModels.MemberVM.MemberVM;
using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer.Models;
using DataLayer.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ACC.Controllers
{
    public class MemberController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRoleRepository _roleRepository;
        private readonly ICompanyRepository _companyRepository;

        public MemberController(UserManager<ApplicationUser> userManager , IRoleRepository roleRepository , ICompanyRepository companyRepository)
        {
            _userManager = userManager;
            _roleRepository = roleRepository;
            _companyRepository = companyRepository;
        }

        public IActionResult Index(int page = 1, string search = "", int pageSize = 10)
        {
            var query = _userManager.Users.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(m => m.UserName.Contains(search));
            }
            var users = query.ToList();
            var totalItems = query.Count();

            var members = query
             .OrderBy(m => m.Id)
             .Skip((page - 1) * pageSize)
             .Take(pageSize)
             .Select(m => new MemberVM
             {
                 Id = m.Id,
                 Name = m.UserName,
                 Status = m.Status,
                 Company = (m.Company).Name ?? "No Company",
                 Role = (m.Role).Name ?? "No Role",
                 AccessLevels = m.AccessLevel, // Adjust based on your model
                 AddedOn = m.AddedOn
             })
             .ToList();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new
                {
                    data = members,
                    totalItems,
                    currentPage = page,
                    pageSize
                });
            }
            ViewBag.Companies = _companyRepository.GetAll();
            ViewBag.Roles = _roleRepository.GetAll();
            return View(members);
        }

        [HttpPost]
        public async Task<IActionResult> InsertMemberAsync(InsertMemberVM memberFromReq)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = memberFromReq.Email.Split("@")[0],
                    Email = memberFromReq.Email,
                    RoleId = memberFromReq.RoleId,
                    CompanyId = memberFromReq.CompanyId,
                    Status = memberFromReq.Status,
                    AccessLevel = new List<AccessLevel>()
                };
                if (memberFromReq.adminAccess) user.AccessLevel.Add(AccessLevel.AccountAdmin);
                if (memberFromReq.excutive) user.AccessLevel.Add(AccessLevel.Excutive);
                if (memberFromReq.standardAccess) user.AccessLevel.Add(AccessLevel.StandardAccess);

                var result = await _userManager.CreateAsync(user, "123Aa_");
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Member added successfully!";
                    return Json(new { success = true });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            TempData["ErrorMessage"] = "Failed to add member!";
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
                    TempData["SuccessMessage"] = "Member deleted successfully!";
                    return Ok();
                }
            }
            TempData["ErrorMessage"] = "Failed to delete member!";
            return NotFound();
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            var member = _userManager.Users.FirstOrDefault(u => u.Id == id);
            if (member != null)
            {
                return Json(new
                {
                    userName = member.UserName,
                    email = member.Email,
                    status = member.Status,
                    company = member.Company?.Name,
                    role = member.Role?.Name,
                    accessLevels = member.AccessLevel
                });
            }
            return Json(new { error = "Member not found" });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAsync(string id,[FromBody] UpdateMemberVM member)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var mmbr = await _userManager.FindByIdAsync(id);
                if (mmbr != null)
                {
                    mmbr.UserName = member.email.Split("@")[0];
                    mmbr.Email = member.email;
                    mmbr.RoleId = member.roleId;
                    mmbr.CompanyId = member.companyId;
                    mmbr.AccessLevel = new List<AccessLevel>();

                    if (member.adminAccess) mmbr.AccessLevel.Add(AccessLevel.AccountAdmin);
                    if (member.excutive) mmbr.AccessLevel.Add(AccessLevel.Excutive);
                    if (member.standardAccess) mmbr.AccessLevel.Add(AccessLevel.StandardAccess);

                    var result = await _userManager.UpdateAsync(mmbr);
                    if (result.Succeeded)
                    {
                        TempData["SuccessMessage"] = "Member updated successfully!";
                        return Json(new { success = true });
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    TempData["ErrorMessage"] = "Failed to update member!";
                    return BadRequest(ModelState);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to update member!";
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetUpdatePartial(string id)
        {
            if (ModelState.IsValid)
            {

                var member = _userManager.Users.FirstOrDefault(u => u.Id == id);
                if (member == null)
                {
                    return NotFound();
                }

                var insertMemberVM = new InsertMemberVM
                {
                    Email = member.Email,
                    RoleId = member.RoleId,
                    CompanyId = member.CompanyId,
                    Status = member.Status,
                    adminAccess = member.AccessLevel?.Contains(AccessLevel.AccountAdmin) ?? false,
                    excutive = member.AccessLevel?.Contains(AccessLevel.Excutive) ?? false,
                    standardAccess = member.AccessLevel?.Contains(AccessLevel.StandardAccess) ?? false
                };
                ViewBag.Companies = new SelectList(_companyRepository.GetAll(), "Id", "Name");
                ViewBag.Roles = new SelectList(_roleRepository.GetAll(), "Id", "Name");

                return PartialView("PartialViews/_UpdateMemberPartialView", insertMemberVM);
            }
            return PartialView("PartialViews/_UpdateMemberPartialView", ModelState);

        }

    }
}
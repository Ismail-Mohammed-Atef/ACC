using ACC.ViewModels.MemberVM;
using ACC.ViewModels.MemberVM.MemberVM;
using ACC.ViewModels.ProjectMembersVM;
using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer;
using DataLayer.Models;
using DataLayer.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace ACC.Controllers
{
    public class ProjectMembersController : Controller
    {
        private readonly IProjetcRepository _projectRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICompanyRepository _companyRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly AppDbContext _context;
        private static int projectId;

        public ProjectMembersController(
            UserManager<ApplicationUser> userManager,
            IRoleRepository roleRepository,
            ICompanyRepository companyRepository,
            AppDbContext context,
            IProjetcRepository projectRepository)
        {
            _userManager = userManager;
            _roleRepository = roleRepository;
            _companyRepository = companyRepository;
            _context = context;
            _projectRepository = projectRepository;
        }

        public IActionResult Index(int id, int page = 1, string search = "", int pageSize = 5)
        {
            if(id == 0)
            {
                id = projectId;
            }
            var projectExists = _context.Projects.Any(p => p.Id == id);
            if (!projectExists)
            {
                TempData["ErrorMessage"] = "Project not found!";
                return RedirectToAction("Index", "Projects");
            }

            var userIdsInProject = _context.ProjectMembers
                .Where(pm => pm.ProjectId == id)
                .Select(pm => pm.MemberId)
                .ToList();

            var query = _userManager.Users
                .Where(u => userIdsInProject.Contains(u.Id))
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(m => m.UserName.Contains(search));
            }

            var totalItems = query.Count();

            var projectMembers = query
                .OrderBy(m => m.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new ProjectMembersVM
                {
                    Id = m.Id,
                    Name = m.UserName,
                    Email = m.Email,
                    Status = m.Status,
                    Company = m.Company != null ? m.Company.Name : "No Company",
                    Role = m.Role != null ? m.Role.Name : "No Role",
                    AddedOn = m.AddedOn
                })
                .ToList();

            //if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            //{
            //    return Json(new
            //    {
            //        data = projectMembers,
            //        totalItems,
            //        currentPage = page,
            //        pageSize
            //    });
            //}

            ViewBag.Members = _userManager.Users.ToList();
            ViewBag.Id = id;
            projectId = id;

            return View(projectMembers);
        }

        [HttpPost]
        public async Task<IActionResult> InsertMemberAsync(InsertMemberVM memberFromReq)
        {
            var memebr = _userManager.Users.Where(u=>u.UserName == memberFromReq.Name).FirstOrDefault();
            if (memebr != null)
            {
                var projectMember = new ProjectMembers()
                {
                    ProjectId = projectId,
                    MemberId = memebr.Id
                };
                if(memebr.Projects is null)
                {
                    memebr.Projects = new List<ProjectMembers>();
                }
                memebr.Projects.Add(projectMember);
                await _userManager.UpdateAsync(memebr);
                
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(string id, int projectId)
        {
            var projectMember = _context.ProjectMembers
                .FirstOrDefault(pm => pm.MemberId == id && pm.ProjectId == projectId);

            if (projectMember != null)
            {
                var member = await _userManager.FindByIdAsync(id);
                if (member != null)
                {
                    var result = await _userManager.DeleteAsync(member);
                    if (result.Succeeded)
                    {
                        _context.ProjectMembers.Remove(projectMember);
                        await _context.SaveChangesAsync();

                        TempData["SuccessMessage"] = "Member deleted successfully!";
                        return Ok();
                    }
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
        public async Task<IActionResult> UpdateAsync(string id, [FromBody] UpdateMemberVM member)
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

                return PartialView("PartialViews/_UpdateProjectMembersPartialView", insertMemberVM);
            }

            return PartialView("PartialViews/_UpdateProjectMembersPartialView", ModelState);
        }
    }
}

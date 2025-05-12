using ACC.ViewModels;
using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer.Models.Enums;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace ACC.Controllers.ProjectDetailsController
{
    public class ProjectCompanyController : Controller
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IProjectActivityRepository _projectActivityRepository;

        public ProjectCompanyController(ICompanyRepository companyRepository, IProjectActivityRepository projectActivityRepository)
        {
            _companyRepository = companyRepository;
            _projectActivityRepository = projectActivityRepository;
        }

        // GET: /ProjectCompany/Index/{id}
        public IActionResult Index(int id, int page = 1, int pageSize = 4)
        {
            var companies = _companyRepository.GetCompaniesInEacProjectWithPrpjectId(id);
            int totalRecords = companies.Count();

            var companyList = companies
                .Select(c => new CompanyVM
                {
                    Id = c.Id,
                    Name = c.Name,
                    Address = c.Address,
                    Description = c.Description,
                    Website = c.Website,
                    PhoneNumber = c.PhoneNumber,
                    SelectedCountry = c.Country,
                    SelectedCompanyType = c.CompanyType
                })
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.Id = id;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            ViewBag.CurrentPage = page;

            return View(companyList);
        }

        // GET: /ProjectCompany/GetCompaniesForProject/{projectId}
        [HttpGet]
        public IActionResult GetCompaniesForProject(int projectId)
        {
            var allCompanies = _companyRepository.GetAll();
            var assignedCompanies = _companyRepository.GetCompaniesInEacProjectWithPrpjectId(projectId).Select(c => c.Id).ToList();

            var companiesForSelection = allCompanies.Select(c => new
            {
                Id = c.Id,
                Name = assignedCompanies.Contains(c.Id) ? $"{c.Name} (Already in project)" : c.Name,
                Disabled = assignedCompanies.Contains(c.Id)
            }).ToList();

            return Json(companiesForSelection);
        }

        // POST: /ProjectCompany/SaveNew
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveNew(CompanyVM model, int id, int? selectedCompanyId)
        {
            if (selectedCompanyId.HasValue)
            {
                // Assign existing company to project
                var company = _companyRepository.GetById(selectedCompanyId.Value);
                if (company == null)
                {
                    return Json(new { success = false, message = "Company not found." });
                }

                _companyRepository.AddCompanyToProject(company.Id, id);
                _projectActivityRepository.AddNewActivity(company);
                return Json(new { success = true, redirect = Url.Action("Index", new { id }) });
            }
            else
            {
                // Create new company and assign to project
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return Json(new { success = false, errors });
                }

                try
                {
                    var company = new Company
                    {
                        Name = model.Name,
                        Address = model.Address,
                        Description = model.Description,
                        Website = model.Website,
                        PhoneNumber = model.PhoneNumber,
                        Country = model.SelectedCountry,
                        CompanyType = model.SelectedCompanyType
                    };

                    _companyRepository.Insert(company);
                    _companyRepository.Save();
                    _companyRepository.AddCompanyToProject(company.Id, id);
                    _projectActivityRepository.AddNewActivity(company);

                    return Json(new { success = true, redirect = Url.Action("Index", new { id }) });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.InnerException?.Message ?? ex.Message });
                }
            }
        }

        // POST: /ProjectCompany/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id, int projectId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = _companyRepository.GetById(id.Value);
            if (company == null)
            {
                return NotFound();
            }

            _companyRepository.RemoveCompanyFromProject(id.Value, projectId);

            if (company.ProjectCompany == null || !company.ProjectCompany.Any())
            {
                _companyRepository.Delete(company);
                _companyRepository.Save();
                _projectActivityRepository.RemoveActivity(company);
            }

            TempData["SuccessMessage"] = "Company removed from project successfully.";
            return RedirectToAction("Index", new { id = projectId });
        }

        // GET: /ProjectCompany/Details/{id}
        public IActionResult Details(int id)
        {
            var company = _companyRepository.GetById(id);
            if (company == null)
            {
                return NotFound();
            }

            return Json(new
            {
                name = company.Name,
                phoneNumber = company.PhoneNumber,
                website = company.Website,
                address = company.Address,
                description = company.Description,
                selectedCountry = company.Country.ToString(),
                selectedCompanyType = company.CompanyType.ToString(),
                companyTypes = Enum.GetValues(typeof(CompanyType))
                    .Cast<CompanyType>()
                    .Select(c => new { Value = c.ToString(), Text = c.ToString() })
                    .ToList(),
                countries = Enum.GetValues(typeof(Country))
                    .Cast<Country>()
                    .Select(c => new { Value = c.ToString(), Text = c.ToString() })
                    .ToList()
            });
        }

        // POST: /ProjectCompany/UpdateCompany
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCompany(int id, [FromBody] UpdatedCompanyVM model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid data." });
            }

            var company = _companyRepository.GetById(id);
            if (company == null)
            {
                return NotFound();
            }

            try
            {
                company.Name = model.Name;
                company.Address = model.Address;
                company.Description = model.Description;
                company.Website = model.Website;
                company.PhoneNumber = model.PhoneNumber;
                company.CompanyType = (CompanyType)Enum.Parse(typeof(CompanyType), model.SelectedCompanyType);
                company.Country = (Country)Enum.Parse(typeof(Country), model.SelectedCountry);

                _companyRepository.Update(company);
                _companyRepository.Save();

                return Json(new { success = true, message = "Company updated successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.InnerException?.Message ?? ex.Message });
            }
        }

        // GET: /ProjectCompany/CheckName
        public IActionResult CheckName(string name)
        {
            var company = _companyRepository.GetCompanyByName(name);
            return Json(company == null);
        }

        // GET: /ProjectCompany/CheckWebsite
        public IActionResult CheckWebsite(string website)
        {
            var company = _companyRepository.GetCompanyByEmail(website);
            return Json(company == null);
        }
    }
}

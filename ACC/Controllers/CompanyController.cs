using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Repository.RepositoryClasses;
using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer.Models.Enums;
using ACC.ViewModels;
using DataLayer.Models;
using NuGet.Protocol.Core.Types;
using Microsoft.AspNetCore.Identity;

namespace ACC.Controllers
{
    public class CompanyController : Controller
    {
        ICompanyRepository companyRepository;

        CompanyVM Company = new CompanyVM();
        public CompanyController(ICompanyRepository companyRepository)
        {
            this.companyRepository = companyRepository;
        }
        public IActionResult Index(int page = 1, int pageSize = 4)
        {
            var query = companyRepository.GetAll();

            int totalRecords = query.Count();

            List<CompanyVM>
    companiesListModel = query.Select(c => new CompanyVM
    {
        Id = c.Id,  
        Name = c.Name,
        Address = c.Address,
        Description = c.Description,
        Country = c.Country,
        Website = c.Website,
        PhoneNumber = c.PhoneNumber,
        CompanyType = c.CompanyType
    }
    ).Skip((page - 1) * pageSize).Take(pageSize).ToList();


            // Pass pagination data to the view
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            ViewBag.CurrentPage = page;

            return View("Index", companiesListModel);

        }

        public IActionResult SearchCompanies(string searchTerm , CompanyType companyType)
        {
            var query = companyRepository.SearchCompanies(searchTerm, companyType);

            var companiesListModel = query.Select(c => new
            {
                Id = c.Id,
                Name = c.Name,
                Address = c.Address,
                Description = c.Description,
                Country = c.Country,
                Website = c.Website,
                PhoneNumber = c.PhoneNumber,
                CompanyType = c.CompanyType.ToString()
            }).ToList();

            return Json(companiesListModel);
        }


        public IActionResult InsertCompany()
        {
            ViewBag.companyList = companyRepository.GetAll();
            return View("InsertCompany");
        }

        [HttpPost]

        public IActionResult SaveNew(Company companyFromRequest)
        {
           
                try
                {
                    companyRepository.Insert(companyFromRequest);
                    companyRepository.Save();
                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);

                }
            

            ViewBag.companytList = companyRepository.GetAll();
            return View("Index", companyFromRequest);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int? id)
        {
            var company = companyRepository.GetById(id.Value);
            if (company == null)
            {
                return NotFound();
            }

           

            companyRepository.Delete(company);
            companyRepository.Save();
            TempData["SuccessMessage"] = "Company deleted successfully.";
            return RedirectToAction("Index");
        }





    }
}

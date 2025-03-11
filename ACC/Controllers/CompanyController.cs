using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Repository.RepositoryClasses;
using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer.Models.Enums;
using ACC.ViewModels;
using DataLayer.Models;
using NuGet.Protocol.Core.Types;

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
        public IActionResult Index(string searchTerm, CompanyType? companyType, int page = 1, int pageSize = 4)
        {
            var query = companyRepository.SearchCompanies(searchTerm, companyType);

            int totalRecords = query.Count();

            List<CompanyVM>
    companiesListModel = query.Select(c => new CompanyVM
    {
        Name = c.Name,
        Address = c.Address,
        Description = c.Description,
        Country = c.Country,
        Website = c.Website,
        PhoneNumber = c.PhoneNumber,
        CompanyType = c.CompanyType
    }
    ).Skip((page - 1) * pageSize).Take(pageSize).ToList();


            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.SearchTerm = searchTerm;

            return View("Index", companiesListModel);

        }

        public IActionResult InsertCompany()
        {
            ViewBag.companyList = companyRepository.GetAll();
            return View("InsertCompany");
        }

        [HttpPost]

        public IActionResult SaveNew(Company companyFromRequest)
        {
            if (ModelState.IsValid == true)
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
            }

            ViewBag.companytList = companyRepository.GetAll();
            return View("InsertCompany", companyFromRequest);
        }

    }
}

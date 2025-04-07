﻿using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer.Models.Enums;
using ACC.ViewModels;
using DataLayer.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using BusinessLogic.Repository.RepositoryClasses;


namespace ACC.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IProjectActivityRepository _projectActivityRepository;

        public CompanyController(ICompanyRepository companyRepository, IProjectActivityRepository projectActivityRepository)
        {
            _companyRepository = companyRepository;
            _projectActivityRepository = projectActivityRepository;
        }

        // GET: Company/Index
        public IActionResult Index(int page = 1, int pageSize = 4)
        {
            var query = _companyRepository.GetAll();

            int totalRecords = query.Count();

            var companiesListModel = query
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

            // Pass pagination data to the view
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            ViewBag.CurrentPage = page;

         
            return View("Index", companiesListModel);
        }


     


        // GET: Company/InsertCompany
        public IActionResult InsertCompany()
        {
            var model = new CompanyVM
            {
                CompanyTypes = Enum.GetValues(typeof(CompanyType)).Cast<CompanyType>().ToList(),
                Countries = Enum.GetValues(typeof(Country)).Cast<Country>().ToList()
            };

            return View("Index", model);
        }

        // POST: Company/SaveNew
        [HttpPost]
        public IActionResult SaveNew(CompanyVM model)
        {
            if (!ModelState.IsValid)
            {
                model.CompanyTypes = Enum.GetValues(typeof(CompanyType)).Cast<CompanyType>().ToList();
                model.Countries = Enum.GetValues(typeof(Country)).Cast<Country>().ToList();
                return RedirectToAction("InsertCompany", model);
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
                _projectActivityRepository.AddNewActivity(company);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
            }

            return RedirectToAction("InsertCompany", model);
        }

        // POST: Company/Delete
        [HttpPost]
        public IActionResult Delete(int? id)
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

            _companyRepository.Delete(company);
            _companyRepository.Save();
            _projectActivityRepository.RemoveActivity(company);

            TempData["SuccessMessage"] = "Company deleted successfully.";
            return RedirectToAction("Index");
        }

       
       


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
                selectedCountry = company.Country,
                selectedCompanyType = company.CompanyType,

                companyTypes = Enum.GetValues(typeof(CompanyType))
                            .Cast<CompanyType>()
                            .Select(c => new
                            {
                                Value = (int)c,     
                                Text = c.ToString() 
                            }).ToList(),



                countries = Enum.GetValues(typeof(Country))
                            .Cast<Country>()
                            .Select(c => new
                            {
                                Value = (int)c, 
                                Text = c.ToString() 
                            }).ToList()



            });
        }



        [HttpPost]
        public IActionResult UpdateCompany( int id ,[FromBody] UpdatedCompanyVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
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



    }
}

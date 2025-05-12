using ACC.ViewModels.TransmittalsVM;
using BusinessLogic.Repository.RepositoryClasses;
using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer.Models.Enums;
using DataLayer.Models.Transmittals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Linq;

namespace ACC.Controllers
{
    public class TransmittalsController : Controller
    {
        private readonly ITransmittalRepository _transmittalRepository;
        private readonly IProjetcRepository _projectRepository;
        private readonly ICompanyRepository _companyRepository;

        public TransmittalsController(ITransmittalRepository transmittalRepository, IProjetcRepository projectRepository, ICompanyRepository companyRepository)
        {
            _transmittalRepository = transmittalRepository;
            _projectRepository = projectRepository;
            _companyRepository = companyRepository;
        }

        public IActionResult Index(int page = 1, int pageSize = 4)
        {
            var query = _transmittalRepository.GetAllWithIncludes();

            int totalRecords = query.Count();

            var transmittalsListModel = query
                .Select(t => new TransmittalVM
                {
                    Id = t.Id,
                    TransmittalId = t.TransmittalId,
                    Title = t.Title,
                    SentDate = t.SentDate,
                    SenderCompanyName = t.SenderCompany.Name,
                    ProjectName = t.Project.Name,
                    Status = t.Status,
                    RecipientCompanyNames = t.Recipients.Select(r => r.RecipientCompany.Name).ToList()
                })
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            ViewBag.CurrentPage = page;

            return View("Index", transmittalsListModel);
        }

        public IActionResult Create()
        {
            var model = new CreateTransmittalVM
            {
                Projects = _projectRepository.GetAll().Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList(),
                Companies = _companyRepository.GetAll().Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList()
            };
            return View("Create", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveNew(CreateTransmittalVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Projects = _projectRepository.GetAll().Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
                model.Companies = _companyRepository.GetAll().Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
                return View("Create", model);
            }

            try
            {
                var transmittal = new Transmittal
                {
                    TransmittalId = model.TransmittalId,
                    Title = model.Title,
                    SentDate = DateTime.Now,
                    ProjectId = model.ProjectId,
                    SenderCompanyId = model.SenderCompanyId,
                    Status = TransmittalStatus.Sent,
                    Recipients = model.RecipientCompanyIds.Select(id => new TransmittalRecipient { RecipientCompanyId = id }).ToList(),
                    Files = new List<TransmittalFile>()
                };

                if (model.Files != null && model.Files.Any())
                {
                    foreach (var file in model.Files)
                    {
                        if (file.Length > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                            transmittal.Files.Add(new TransmittalFile { FileName = fileName, FilePath = "/uploads/" + fileName });
                        }
                    }
                }

                _transmittalRepository.Insert(transmittal);
                _transmittalRepository.Save();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                model.Projects = _projectRepository.GetAll().Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
                model.Companies = _companyRepository.GetAll().Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
                return View("Create", model);
            }
        }
    }
}

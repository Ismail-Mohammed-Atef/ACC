using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer.Models;
using ACC.ViewModels;

namespace ACC.Controllers
{
    public class ProjectIssueController : Controller
    {
        private readonly IIssueService _issueService;

        public ProjectIssueController(IIssueService issueService)
        {
            _issueService = issueService;
        }

        public IActionResult Index(int? projectId)
        {
            List<Issue> issues;
            if (projectId.HasValue)
            {
                issues = _issueService.GetIssuesByProjectId(projectId.Value);
            }
            else
            {
                issues = _issueService.GetAllIssues();
            }
            var issueViewModels = issues.Select(i => new ProjectIssueVM
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                Category = i.Category,
                Type = i.Type,
                Priority = i.Priority,
                Status = i.Status,
                ProjectId = i.ProjectId
            }).ToList();
            return View(issueViewModels);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProjectIssueVM model)
        {
            if (ModelState.IsValid)
            {
                var issue = new Issue
                {
                    Title = model.Title,
                    Description = model.Description,
                    Category = model.Category,
                    Type = model.Type,
                    Priority = model.Priority,
                    Status = model.Status,
                    ProjectId = model.ProjectId
                };
                _issueService.CreateIssue(issue);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var issue = _issueService.GetIssueById(id);
            var model = new ProjectIssueVM
            {
                Id = issue.Id,
                Title = issue.Title,
                Description = issue.Description,
                Category = issue.Category,
                Type = issue.Type,
                Priority = issue.Priority,
                Status = issue.Status,
                ProjectId = issue.ProjectId
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(ProjectIssueVM model)
        {
            if (ModelState.IsValid)
            {
                var issue = new Issue
                {
                    Id = model.Id,
                    Title = model.Title,
                    Description = model.Description,
                    Category = model.Category,
                    Type = model.Type,
                    Priority = model.Priority,
                    Status = model.Status,
                    ProjectId = model.ProjectId
                };
                _issueService.UpdateIssue(issue);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            _issueService.DeleteIssue(id);
            return RedirectToAction("Index");
        }
    }
}
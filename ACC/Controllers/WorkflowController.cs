﻿using ACC.Services;
using ACC.ViewModels.WorkflowVM;
using BusinessLogic.Repository.RepositoryClasses;
using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer.Models;
using DataLayer.Models.Enums;
using Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace ACC.Controllers
{
    public class WorkflowController : Controller
    {
        private readonly IWorkflowRepository _workflowRepository;
        private readonly IWorkFlowStepRepository _workFlowStepRepository;
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly WorkflowStepsUsersService _workflowStepsUsersService;
        private readonly ReviewStepUsersService _reviewStepUsersService;
        private readonly FolderService _folderService;

        public WorkflowController(IWorkflowRepository workflowRepository , IWorkFlowStepRepository workFlowStepRepository, UserManager<ApplicationUser> userManager , WorkflowStepsUsersService workflowStepsUsersService, ReviewStepUsersService reviewStepUsersService , FolderService folderService)
        {
            _workflowRepository = workflowRepository;
            _workFlowStepRepository = workFlowStepRepository;
            UserManager = userManager;
            _workflowStepsUsersService = workflowStepsUsersService;
            _reviewStepUsersService = reviewStepUsersService;
            _folderService = folderService;
        }

        public IActionResult Index(int id , int page = 1, int pageSize = 4)
        {

            var query = _workflowRepository.GetAllWithSteps(id);

            if (query == null)
            {
                throw new InvalidOperationException("The workflow repository returned null. Make sure it's returning a valid collection.");
            }

            int totalRecords = query.Count();

            var WorkflowTemplates = query
                .Select(c => new WorkflowTemplateViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Steps = c.Steps.Select(s => new WorkflowStepInputViewModel
                    {
                        StepOrder = s.StepOrder,
                        TimeAllowedInDays = s.TimeAllowed
                    }).ToList()
                })
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.Id = id;

            return View("Index", WorkflowTemplates);
        }

        public IActionResult NewWorkflow(int stepCount , int proId)
        {
            var vm = new WorkflowTemplateViewModel();
            vm.ReviewersType = Enum.GetValues(typeof(ReviewersType)).Cast<ReviewersType>().ToList();
            vm.AllFolders = _folderService.GetFolderTree();
           

            for (int i = 0; i < stepCount; i++)
            {
                vm.Steps.Add(new WorkflowStepInputViewModel());
            }
            vm.applicationUsers = UserManager.Users.ToList();

            ViewBag.MultiReviwerOptions = Enum_Helper.GetEnumSelectListWithDisplayNames<MultiReviewerOptions>();
            ViewBag.Id = proId;



            return View("NewWorkflow", vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTemplate(WorkflowTemplateViewModel vm)
        {
            var template = new WorkflowTemplate
            {
               
                ProjectId = vm.proId,
                Name = vm.Name,
                Description = vm.Description,
                StepCount = vm.Steps.Count,
                CopyApprovedFiles = vm.CopyApprovedFiles,
                DestinationFolderId = vm.SelectedDistFolderId
                
            };

            foreach (var step in vm.Steps)
            {
             
                

                var stepTemplate = new WorkflowStepTemplate
                {
                    StepOrder = step.StepOrder,
                    TimeAllowed = step.TimeAllowedInDays,
                    ReviewersType = step.SelectedReviewersType,
                    MinReviewers = step.MinReviewers,

                };







                if (step.SelectedOption == "Every key reviewer must review this step")
                {
                    stepTemplate.MultiReviewerOptions = MultiReviewerOptions.EveryOne;
                }
                else
                {
                    stepTemplate.MultiReviewerOptions = MultiReviewerOptions.MinimumNumber;
                }

                template.Steps.Add(stepTemplate);
            }

            _workflowRepository.Insert(template);
            _workflowRepository.Save();

            var savedSteps = template.Steps.OrderBy(s => s.StepOrder).ToList();

            for (int i = 0; i < vm.Steps.Count; i++)
            {
                var step = vm.Steps[i];
                var savedStep = savedSteps[i]; 

                foreach (var userId in step.AssignedUsersIds)
                {
                    var user = await UserManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        WorkflowStepUser workflowStepUser = new WorkflowStepUser()
                        {
                            StepId = savedStep.Id,
                            UserId = user.Id,
                        };

                        _workflowStepsUsersService.Insert(workflowStepUser);
                    }
                }
            }

            

            _workflowStepsUsersService.Save();


            return RedirectToAction("Index", new { id = vm.proId });
        }

       [HttpGet]
       public async Task<IActionResult> EditWorkflow(int id)
        {
            var workflowFromDB = _workflowRepository.GetById(id);
            if (workflowFromDB == null)
            {
                return NotFound();
            }

            var vm = new WorkflowTemplateViewModel
            {
                Id= workflowFromDB.Id,
                proId = workflowFromDB.ProjectId,
                Name = workflowFromDB.Name,
                Description = workflowFromDB.Description,
                CopyApprovedFiles = workflowFromDB.CopyApprovedFiles,
                SelectedDistFolderId = workflowFromDB.DestinationFolderId,
                ReviewersType = Enum.GetValues(typeof(ReviewersType)).Cast<ReviewersType>().ToList(),
                AllFolders = _folderService.GetFolderTree(),
                applicationUsers = UserManager.Users.ToList()
            };

            var stepTemplates = workflowFromDB.Steps.OrderBy(s => s.StepOrder).ToList();
            foreach (var stepTemplate in stepTemplates)
            {
                var assignedUsers = _workflowStepsUsersService.GetByStepId(stepTemplate.Id);
                var stepVM = new WorkflowStepInputViewModel
                {
                    StepOrder = stepTemplate.StepOrder,
                    TimeAllowedInDays = stepTemplate.TimeAllowed,
                    SelectedReviewersType = stepTemplate.ReviewersType,
                    MinReviewers = stepTemplate.MinReviewers,
                    SelectedOption = stepTemplate.MultiReviewerOptions == MultiReviewerOptions.EveryOne
                                     ? "Every key reviewer must review this step"
                                     : "Minimum number of reviewers",
                    AssignedUsersIds = assignedUsers.Select(u => u.UserId).ToList()
                };

                vm.Steps.Add(stepVM);
            }

            ViewBag.MultiReviwerOptions = Enum_Helper.GetEnumSelectListWithDisplayNames<MultiReviewerOptions>();
            ViewBag.Id = workflowFromDB.ProjectId;

            return View("EditWorkflow", vm);
        }

        [HttpPost]
        public async Task<IActionResult> SaveWorkflow(WorkflowTemplateViewModel vm)
        {
            var template = _workflowRepository.GetById((int)vm.Id);

            if (template == null)
                return NotFound();

            template.Name = vm.Name;
            template.Description = vm.Description;
            template.CopyApprovedFiles = vm.CopyApprovedFiles;
            template.DestinationFolderId = vm.SelectedDistFolderId;

            var StepsFromVM = vm.Steps.OrderBy(s => s.StepOrder).ToList(); 
            var stepsFromDB = template.Steps.OrderBy(s => s.StepOrder).ToList();

            for (int i=0; i< template.Steps.Count; i++)
            {
                var StepFromVM = StepsFromVM[i];
                stepsFromDB[i].StepOrder = StepFromVM.StepOrder;
                stepsFromDB[i].TimeAllowed = StepFromVM.TimeAllowedInDays;
                stepsFromDB[i].ReviewersType = StepFromVM.SelectedReviewersType;
                stepsFromDB[i].MinReviewers = StepFromVM.MinReviewers;
                stepsFromDB[i].MultiReviewerOptions = StepFromVM.SelectedOption == "Every key reviewer must review this step"
                                            ? MultiReviewerOptions.EveryOne
                                            : MultiReviewerOptions.MinimumNumber;
            }

        

            for (int i = 0; i < vm.Steps.Count; i++)
            {
                var step = StepsFromVM[i];
                var savedStep = stepsFromDB[i];


                var OldStepUsers = _workflowStepsUsersService.GetByStepId(savedStep.Id);

                foreach (var item in OldStepUsers)
                {

                    _workflowStepsUsersService.Delete(item);

                }

                _workflowStepsUsersService.Save();
                savedStep.workflowStepUsers.Clear();
                foreach (var userId in step.AssignedUsersIds)
                {
                    var user = await UserManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        WorkflowStepUser workflowStepUser = new WorkflowStepUser()
                        {
                            StepId = savedStep.Id,
                            UserId = user.Id,
                        };
                        _workflowStepsUsersService.Insert(workflowStepUser);
                        savedStep.workflowStepUsers.Add(workflowStepUser);
                    }
                }
            }

            

            _workflowStepsUsersService.Save();

            var reviews = template.Reviews;
            foreach (var review in reviews)
            {


                for (int i = 0; i < vm.Steps.Count; i++)
                {
                    var step = StepsFromVM[i];
                    var savedStep = stepsFromDB[i];

                    var OldStepUsers = _reviewStepUsersService.GetByStepId(savedStep.Id);

                    foreach (var item in OldStepUsers)
                    {

                        _reviewStepUsersService.Delete(item);

                    }
                    _reviewStepUsersService.Save();
                    foreach (var userId in step.AssignedUsersIds)
                    {
                        var user = await UserManager.FindByIdAsync(userId);
                        if (user != null)
                        {
                            ReviewStepUser ReviewStepUser = new ReviewStepUser()
                            {
                                StepId = savedStep.Id,
                                UserId = user.Id,
                                ReviewId = review.Id
                            };

                            _reviewStepUsersService.Insert(ReviewStepUser);
                        }
                    }
                }


            }

            _workflowRepository.Save();

            return RedirectToAction("Index", new { id = vm.proId });
        }



    }
}

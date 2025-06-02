using ACC.Services;
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
        private readonly FolderService _folderService;

        public WorkflowController(IWorkflowRepository workflowRepository , IWorkFlowStepRepository workFlowStepRepository, UserManager<ApplicationUser> userManager , WorkflowStepsUsersService workflowStepsUsersService , FolderService folderService)
        {
            _workflowRepository = workflowRepository;
            _workFlowStepRepository = workFlowStepRepository;
            UserManager = userManager;
            _workflowStepsUsersService = workflowStepsUsersService;
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



    



    }
}


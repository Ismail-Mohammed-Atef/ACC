﻿using System.Linq.Expressions;
using System.Threading.Tasks;
using ACC.Services;
using ACC.ViewModels.ReviewsVM;
using BusinessLogic.Repository.RepositoryClasses;
using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer;
using DataLayer.Models;
using DataLayer.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace ACC.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IWorkflowRepository _workflowRepository;
        private readonly IWorkFlowStepRepository _workFlowStepRepo;
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly FolderService FolderService;
        private readonly ReviewDocumentService ReviewDocumentService;
        private readonly ReviewFolderService ReviewFolderService;
        private readonly WorkflowStepsUsersService WorkflowStepUserService;
        private readonly ReviewStepUsersService ReviewStepUsersService;

        public ReviewsController(IReviewRepository reviewRepo, IWorkflowRepository workflowRepo, IWorkFlowStepRepository workFlowStepRepo, FolderService folderService, ReviewDocumentService reviewDocumentService, ReviewFolderService reviewFolderService ,WorkflowStepsUsersService workflowStepsUsersService, ReviewStepUsersService reviewStepUsersService , UserManager<ApplicationUser> userManager)
        {
            _reviewRepository = reviewRepo;
            _workflowRepository = workflowRepo;
            _workFlowStepRepo = workFlowStepRepo;
            FolderService = folderService;
            ReviewDocumentService = reviewDocumentService;
            ReviewFolderService = reviewFolderService;
            WorkflowStepUserService= workflowStepsUsersService;
            ReviewStepUsersService = reviewStepUsersService;
            UserManager = userManager;
        }
        [Authorize]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 4)
        
        
        {
            var CurrentUser = await UserManager.GetUserAsync(User);
            var query = _reviewRepository.GetCurrentStepUserReviews(CurrentUser.Id);

            int totalRecords = query.Count();




            var ReviewListModel = query
                .Select(c => new ReviewVM
                {
                    Id = c.Id,
                    Name = c.Name,
                    WorkflowName = _workflowRepository.GetAll().FirstOrDefault(wf => wf.Id == c.WorkflowTemplateId).Name,
                    FinalReviewStatus = c.FinalReviewStatus.ToString(),
                    CreatedAt = c.CreatedAt.ToString(),
                    Initiator = c.InitiatorUserId,
                    CurrentUserId = CurrentUser.Id,
                })
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Pass pagination data to the view
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            ViewBag.CurrentPage = page;

            ViewBag.CreateReviewVM = new CreateReviewVM
            {
                WorkflowTemplates = _workflowRepository.GetAll().ToList(),
                FinalReviewStatuses = Enum.GetValues(typeof(FinalReviewStatus)).Cast<FinalReviewStatus>().ToList(),
                AllFolders = FolderService.GetFolderTree(),
            };

            return View("Index", ReviewListModel);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> SaveReviewAsync(CreateReviewVM model)
        {

            var Templatesteps = _workflowRepository.GetById(model.SelectedWorkflowId).Steps;
            var CurrentUser = await UserManager.GetUserAsync(User);


            try
            {

                var Review = new Review
                {
                    Name = model.Name,
                    FinalReviewStatus = model.SelectedFinalReviewStatus,
                    WorkflowTemplate = _workflowRepository.GetById(model.SelectedWorkflowId),
                    WorkflowTemplateId = model.SelectedWorkflowId,
                    CurrentStepId = _workflowRepository.GetById(model.SelectedWorkflowId).Steps[0].Id,
                    CreatedAt = DateTime.Now,
                    InitiatorUserId = CurrentUser.Id,

                };
                _reviewRepository.Insert(Review);
                _reviewRepository.Save();


                if(model.SelectedDocumentIds != null)
                {
                    foreach (var id in model.SelectedDocumentIds)
                    {
                        var ReviewDocument = new ReviewDocument
                        {
                            ReviewId = Review.Id,
                            DocumentId = id,
                        };

                        ReviewDocumentService.Insert(ReviewDocument);
                        ReviewDocumentService.Save();


                    }

                }
               
                if(model.SelectedFolderIds !=null)
                {
                    foreach (var id in model.SelectedFolderIds)
                    {
                        var ReviewFolder = new ReviewFolder
                        {
                            ReviewId = Review.Id,
                            FolderId = id,
                        };

                        ReviewFolderService.Insert(ReviewFolder);
                        ReviewFolderService.Save();


                    }

                }

                List<int> StepsId = new List<int>();
                foreach(var item in Review.WorkflowTemplate.Steps)
                {
                    StepsId.Add(item.Id);
                }

                List<WorkflowStepUser> workflowStepUsers = new List<WorkflowStepUser>();
                foreach(var item in StepsId)
                {
                    List<WorkflowStepUser> workflowStepUsers2 = WorkflowStepUserService.GetByStepId(item).ToList();
                    foreach(var item2 in workflowStepUsers2)
                    {
                        workflowStepUsers.Add(item2);

                    }

                }

                foreach(var item in workflowStepUsers)
                {
                    var ReviewStepUser = new ReviewStepUser()
                    {
                        ReviewId = Review.Id,
                        StepId = item.StepId,
                        UserId = item.UserId,
                    };
                    ReviewStepUsersService.Insert(ReviewStepUser);
                    ReviewStepUsersService.Save();
                }


                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
            }

            return RedirectToAction("CreateReview", model);
        }


        public async Task<IActionResult> Approve(int Id)
        {
            Review ReviewFromDB = _reviewRepository.GetReviewById(Id);

            int CurrentReviewStep = ReviewFromDB.CurrentStep.StepOrder.Value;
            int NextReviewStep = CurrentReviewStep + 1;
            int StepsCount = _workflowRepository.GetById(ReviewFromDB.WorkflowTemplateId).Steps.Count();
            var StepMultiReviewerOption = ReviewFromDB.CurrentStep.MultiReviewerOptions;
            var StepReviewerType = ReviewFromDB.CurrentStep.ReviewersType;

            var CurrentUser = await UserManager.GetUserAsync(User);
            var CurrntStep = ReviewFromDB.CurrentStep;
            ReviewStepUsersService.Get(CurrentUser.Id, CurrntStep.Id , ReviewFromDB.Id).IsApproved = true;
            ReviewStepUsersService.Save();

            bool Advance = true;

            if (NextReviewStep <= StepsCount)
            {
                if(StepReviewerType == (ReviewersType)0)
                {
                    Advance = true;
                }
                
                else if (StepReviewerType == (ReviewersType)1) 
                {
                    if(StepMultiReviewerOption == (MultiReviewerOptions)1)
                    {
                       var StepUsers = ReviewStepUsersService.GetByStepId(CurrntStep.Id);
                       foreach (var item in StepUsers)
                        {
                            if (item.IsApproved != true)
                            {
                                Advance = false;
                                break;
                            }
                        }

                    }
                    else if (StepMultiReviewerOption == (MultiReviewerOptions)2)
                    {
                        var StepUsers = ReviewStepUsersService.GetByStepId(CurrntStep.Id);
                        int counter = 0;

                        foreach(var item in StepUsers)
                        {
                            if(item.IsApproved==true)
                                { counter++; }
                        }

                        if (counter != CurrntStep.MinReviewers)
                        {
                            Advance=false;
                        }
                    }
                    
                }

                if(Advance)
                {
                    ReviewFromDB.CurrentStepId = _workflowRepository.GetById(ReviewFromDB.WorkflowTemplateId).Steps.FirstOrDefault(r => r.StepOrder == NextReviewStep).Id;
                    _reviewRepository.Save();
                }
               

            }
            else
            {
                //Apply action
            }

            return RedirectToAction("Index");
        }

        public IActionResult Reject(int Id)
        {
            Review ReviewFromDB = _reviewRepository.GetById(Id);

            int CurrentReviewStep = (int)_workFlowStepRepo.GetById(ReviewFromDB.CurrentStepId.Value).StepOrder;
            int PreviousReviewStep = CurrentReviewStep - 1;
            int StepsCount = _workflowRepository.GetById(ReviewFromDB.WorkflowTemplateId).Steps.Count();

            
            if (PreviousReviewStep > 0)
            {
                ReviewFromDB.CurrentStepId = _workflowRepository.GetById(ReviewFromDB.WorkflowTemplateId).Steps.FirstOrDefault(r => r.StepOrder == PreviousReviewStep).Id;
                _reviewRepository.Save();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {

            var CurrentUser = await UserManager.GetUserAsync(User);
            var review = _reviewRepository.GetReviewById(id);

            if (review == null)
                return NotFound();

            var DocumentsListVM = review.ReviewDocuments.Select(rd => new DocumentUponAction
            {
                Id = rd.Document.Id,
                Name = rd.Document.Name,
            }).ToList();

            ViewBag.ReviewName = review.Name;
            ViewBag.ReviewId = review.Id;
            ViewBag.CurrentUserId = CurrentUser.Id;
            ViewBag.Initiator = review.InitiatorUser;
            return View("Details" , DocumentsListVM);
        }

        [HttpPost]
        public ActionResult Submit(List<DocumentUponAction> DocumentList , int ReviewId)
        {
            Review ReviewFromDB = _reviewRepository.GetById(ReviewId);

            foreach(var doc in DocumentList)
            {
                if( doc.IsApproved == false)
                {
                    return RedirectToAction("Reject" , new { Id = ReviewId });
                }
            }



            return RedirectToAction("Approve" , new { Id = ReviewId });

        }




    }
}


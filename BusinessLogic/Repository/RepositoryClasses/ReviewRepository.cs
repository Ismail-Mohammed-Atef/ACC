
using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer;
using DataLayer.Models;
using DataLayer.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Repository.RepositoryClasses
{
    public class ReviewRepository : GenericRepository<Review> , IReviewRepository
    {
        AppDbContext Context;
        public ReviewRepository(AppDbContext context) : base(context)
        {
            Context = context; ;
        }

        public WorkflowTemplate GetWorkflowTemp(int WorkflowId)
        {
            return Context.WorkflowTemplates.Where(wf => wf.Id == WorkflowId).FirstOrDefault();
        }
        public WorkflowStepTemplate GetWorkflowStepTemp(int WorkflowStepId)
        {
            return Context.WorkflowStepTemplates.Where(wf => wf.Id == WorkflowStepId).FirstOrDefault();
        }


        public List<Review> GetCurrentStepUserReviews(string userId)
        {
            return Context.Reviews
                .Include(r => r.CurrentStep)
                .ThenInclude(s => s.workflowStepUsers)
                .Where(r =>
                    r.FinalReviewStatus == FinalReviewStatus.Pending &&
                    (r.InitiatorUserId == userId || r.CurrentStep.workflowStepUsers.Any(i=>i.UserId == userId)))
                .ToList();
        }


        public List<Folder> GetFolders(List<int> foldersIds)
        {
            return Context.Folders.Where(f => foldersIds.Contains(f.Id)).ToList();
        }


        public List<Document> GetDocuments(List<int> documentsIds)
        {
            return Context.Documents.Where(d => documentsIds.Contains(d.Id)).ToList();
        }

        public Review GetReviewById(int Id)
        {
            return Context.Reviews
                .Include(r => r.ReviewDocuments)
                    .ThenInclude(rd => rd.Document)
                .Include(r => r.ReviewFolders)
                    .ThenInclude(rf => rf.Folder) 
                .Include(r => r.CurrentStep)
                .FirstOrDefault(r => r.Id == Id);
        }

        public bool CheckEveryOneApproval()
        {
            return Context.ReviewDocuments.Any(r => r.IsApproved == false);
        }



    }

}


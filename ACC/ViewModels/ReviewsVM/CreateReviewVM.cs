using DataLayer.Models.Enums;
using DataLayer.Models;
using System.Collections.Generic;
using ACC.ViewModels.WorkflowVM;

namespace ACC.ViewModels.ReviewsVM
{
    public class CreateReviewVM
    {
        
        public int proId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }

        public int SelectedWorkflowId { get; set; }

        public FinalReviewStatus SelectedFinalReviewStatus { get; set; }

        public List<WorkflowTemplate> WorkflowTemplates { get; set; }
        public List<FinalReviewStatus> FinalReviewStatuses { get; set; }

        public List<FolderWithDocumentsVM>? AllFolders { get; set; }

        public List<int>? SelectedFolderIds { get; set; }
        public List<int>? SelectedDocumentIds { get; set; }
    }
}

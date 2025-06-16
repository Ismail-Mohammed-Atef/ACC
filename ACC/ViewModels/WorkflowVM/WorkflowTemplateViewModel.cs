﻿using DataLayer.Models;
using DataLayer.Models.Enums;

namespace ACC.ViewModels.WorkflowVM
{
    public class WorkflowTemplateViewModel
    {

        public int proId { get; set; }
        public int? Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<ProjectReviewersVM> Reviewers { get; set; } = new List<ProjectReviewersVM>();

        public string SelectedAppUserId { get; set; }


        public List<FolderVM>? AllFolders { get; set; }

        
        

        public List<WorkflowStepInputViewModel> Steps { get; set; } = new List<WorkflowStepInputViewModel>();



        public List<ReviewersType>? ReviewersType { get; set; } = Enum.GetValues(typeof(ReviewersType)).Cast<ReviewersType>().ToList();

        public List<ApplicationRole> ProjectPositions { get; set; } = new List<ApplicationRole>();




        public int? SelectedDistFolderId { get; set; }

        public bool CopyApprovedFiles { get; set; } 

    }

}

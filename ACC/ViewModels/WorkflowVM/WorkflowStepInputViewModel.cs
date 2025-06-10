using DataLayer.Models;
using DataLayer.Models.Enums;

namespace ACC.ViewModels.WorkflowVM
{
    public class WorkflowStepInputViewModel
    {
        public int? StepOrder { get; set; }

        public int TimeAllowedInDays { get; set; }

        public List<string> AssignedUsersIds { get; set; } = new List<string>();

        public ReviewersType? SelectedReviewersType { get; set; }
        public string? SelectedOption { get; set; }

        public int? MinReviewers { get; set; }

        



    }
}

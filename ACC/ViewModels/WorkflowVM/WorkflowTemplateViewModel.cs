using DataLayer.Models;
using DataLayer.Models.Enums;

namespace ACC.ViewModels.WorkflowVM
{
    public class WorkflowTemplateViewModel
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<ApplicationUser> applicationUsers { get; set; }

        public string SelectedAppUserId { get; set; }




        public List<WorkflowStepInputViewModel> Steps { get; set; } = new List<WorkflowStepInputViewModel>();



        public List<ReviewersType>? ReviewersType { get; set; } = Enum.GetValues(typeof(ReviewersType)).Cast<ReviewersType>().ToList();


    }

}

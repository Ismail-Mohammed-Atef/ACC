using DataLayer.Models;
using DataLayer.Models.Enums;

namespace ACC.ViewModels.ReviewsVM
{
    public class ReviewVM
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        
        public string? WorkflowName { get; set; }

        public string? FinalReviewStatus { get; set; } = "Pending";

        public string? CreatedAt { get; set; } 

        public string Initiator { get; set; }

        public string CurrentUserId { get; set; }




    }
}  

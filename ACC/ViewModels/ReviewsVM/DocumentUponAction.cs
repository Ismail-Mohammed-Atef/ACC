using DataLayer.Models.Enums;

namespace ACC.ViewModels.ReviewsVM
{
    public class DocumentUponAction
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Comment { get; set; }

        public FinalReviewStatus State { get; set; } = FinalReviewStatus.Pending;

        public bool? IsApproved { get; set; }
    }
}

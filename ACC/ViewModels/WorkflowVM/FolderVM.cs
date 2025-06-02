using ACC.ViewModels.ReviewsVM;

namespace ACC.ViewModels.WorkflowVM
{
    public class FolderVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<FolderVM> Children { get; set; } = new();
    }
}

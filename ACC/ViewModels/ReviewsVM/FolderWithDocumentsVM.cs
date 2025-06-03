using ACC.ViewModels.ReviewsVM;

namespace ACC.ViewModels.WorkflowVM
{
    public class FolderWithDocumentsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<DocumentVM> Documents { get; set; } = new();
        public List<FolderWithDocumentsVM> Children { get; set; } = new();
    }
}

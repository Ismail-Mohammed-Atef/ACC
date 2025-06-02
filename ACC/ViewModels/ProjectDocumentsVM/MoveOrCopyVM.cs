namespace ACC.ViewModels.ProjectDocumentsVM
{
    public class MoveOrCopyVM
    {
        public int DocumentId { get; set; }
        public int TargetFolderId { get; set; }
        public string ActionType { get; set; }
    }
}

namespace ACC.ViewModels.IssueCommentVM
{
    public class IssueCommentVM
    {
        public int Id { get; set; }
        public int IssueId { get; set; }
        public string Content { get; set; }
        public string? ImagePath { get; set; }

        public string AuthorName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

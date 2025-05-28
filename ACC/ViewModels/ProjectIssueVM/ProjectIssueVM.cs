using DataLayer.Models.Enums;
using Microsoft.AspNetCore.Http;
namespace ACC.ViewModels
{
    public class ProjectIssueVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IssueCategory Category { get; set; }
        public IssueType Type { get; set; }
        public IssuePriority Priority { get; set; }
        public IssueStatus Status { get; set; }
        public int ProjectId { get; set; }
        public IFormFile Attachment { get; set; }
        public int? DocumentId { get; set; }

    }
}
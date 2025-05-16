using System;

namespace DataLayer.Models
{
    public class Issue : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Enums.IssueCategory Category { get; set; }
        public Enums.IssueType Type { get; set; }
        public Enums.IssuePriority Priority { get; set; }
        public Enums.IssueStatus Status { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace DataLayer.Models
{
    public class Issue : BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Enums.IssueCategory Category { get; set; }
        public Enums.IssueType Type { get; set; }
        public Enums.IssuePriority Priority { get; set; }
        public Enums.IssueStatus Status { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public int? DocumentId { get; set; }
        public Document Document { get; set; }

        public string? InitiatorID { get; set; }
        public ApplicationUser Initiator { get; set; }

        public List<IssueReviwers>? IssueReviwers { get; set; }

        public List<IssueComment> Comments { get; set; } = new List<IssueComment>();
    }
}

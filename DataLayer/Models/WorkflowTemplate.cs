using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models.Enums;

namespace DataLayer.Models
{
    public class WorkflowTemplate : BaseEntity
    {

        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int StepCount { get; set; }
        public List<WorkflowStepTemplate> Steps { get; set; } = new List<WorkflowStepTemplate>();

        public List<Review> Reviews { get; set; } = new List<Review>();

        public bool CopyApprovedFiles { get; set; }

        public int? DestinationFolderId { get; set; }

        

    }

}

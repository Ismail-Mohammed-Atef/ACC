using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class WorkflowStepUser
    {
        public int StepId { get; set; }
        public WorkflowStepTemplate Step { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public bool? IsApproved { get; set; }
    }
}

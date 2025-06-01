using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class IssueReviwers
    {
        public int IssueId { get; set; }
        public Issue Issue { get; set; }

        public string ReviewerId { get; set; }

        public ApplicationUser Reviewer { get; set; }

        public bool? IsApproved { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class ProjectMembers:BaseEntity
    {
        public string MemberId { get; set; }
        public ApplicationUser? Member { get; set; }
        public int ProjectId { get; set; }
        public Project? Project { get; set; }

    }
}

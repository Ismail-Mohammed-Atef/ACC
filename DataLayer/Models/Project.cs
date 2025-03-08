using DataLayer.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Project : BaseEntity
    {
        public string Name { get; set; }
        public string ProjectNumber { get; set; }
        public ProjectType ProjectType { get; set; }
        public string Address { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double ProjectValue { get; set; }
        public Currency Currency { get; set; }

        public ICollection<ProjectMembers>? Members { get; set; }

    }
}

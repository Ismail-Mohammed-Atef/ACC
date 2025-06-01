  using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class ProjectCompany : BaseEntity
    {
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
        public int CompanyId { get; set; }
        public Company? Company { get; set; }
    }
}

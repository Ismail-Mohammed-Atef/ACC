using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class ReviewFolder 
    {
        public int ReviewId { get; set; }
        public Review Review { get; set; }

        public int FolderId { get; set; }
        public Folder Folder { get; set; }

        public string? Comment { get; set; }
        public string? Status { get; set; }

        public bool? IsApproved { get; set; }
    }
}

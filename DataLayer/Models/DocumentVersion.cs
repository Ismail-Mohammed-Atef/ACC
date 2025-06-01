using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class DocumentVersion : BaseEntity
    {
        public int DocumentId { get; set; }
        public  Document Document { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; }
        public string UploadedBy { get; set; }
        public int VersionNumber { get; set; }
    }
}

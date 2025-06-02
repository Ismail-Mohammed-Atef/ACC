using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Document : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public int FolderId { get; set; }
        public Folder Folder { get; set; }
        public int ProjectId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public List<DocumentVersion> Versions { get; set; }

        public List<ReviewDocument> ReviewDocuments { get; set; } = new List<ReviewDocument>();
    }
}

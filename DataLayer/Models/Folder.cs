using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Folder : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentFolderId { get; set; }
        public int ProjectId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public List<Folder> SubFolders { get; set; }
        public List<Document> Documents { get; set; }

        public List<ReviewFolder> ReviewFolders { get; set; } = new List<ReviewFolder>();
    }
}

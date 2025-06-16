using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class IssueComment : BaseEntity
    {
        public int Id { get; set; }
        public int IssueId { get; set; }
        public string AuthorId { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public Issue Issue { get; set; }
        public ApplicationUser Author { get; set; } // Assuming Identity User class
    }
}

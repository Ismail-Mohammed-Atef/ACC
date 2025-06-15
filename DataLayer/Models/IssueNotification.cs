using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class IssueNotification : BaseEntity
    {
        public int Id { get; set; }
        public string ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; }

        public int IssueId { get; set; }
        public Issue Issue { get; set; }

        public string Message { get; set; }
        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}

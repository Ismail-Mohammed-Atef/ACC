using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models.Enums;

namespace DataLayer.Models.Transmittals
{
    public class Transmittal : BaseEntity
    {


        public string TransmittalId { get; set; } // Unique identifier, e.g., TR-001
        public string Title { get; set; }
        public DateTime SentDate { get; set; }
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
        public int SenderCompanyId { get; set; }
        public Company? SenderCompany { get; set; }
        public TransmittalStatus Status { get; set; }
        public ICollection<TransmittalRecipient>? Recipients { get; set; }
        public ICollection<TransmittalFile>? Files { get; set; }





    }
}

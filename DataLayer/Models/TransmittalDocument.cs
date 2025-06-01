using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class TransmittalDocument : BaseEntity
    {

        public int TransmittalId { get; set; }
        public Transmittal Transmittal { get; set; }
        public int DocumentVersionId { get; set; }
        public DocumentVersion DocumentVersion { get; set; }
        public string Notes { get; set; }

    }
}

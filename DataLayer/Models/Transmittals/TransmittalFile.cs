using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models.Transmittals
{
    public class TransmittalFile : BaseEntity
    {

        public int TransmittalId { get; set; }
        public Transmittal? Transmittal { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

    
    }
}

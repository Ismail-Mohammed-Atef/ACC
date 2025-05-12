using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models.Transmittals
{
    public class TransmittalRecipient 
    {
        public int TransmittalId { get; set; }
        public Transmittal? Transmittal { get; set; }


        public int RecipientCompanyId { get; set; }
        public Company? RecipientCompany { get; set; }

    }
}

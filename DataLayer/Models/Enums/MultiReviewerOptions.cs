using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models.Enums
{
    public enum MultiReviewerOptions
    {
        [Display(Name = "Every key reviewer must review this step")]
        EveryOne = 1,

        [Display(Name = "A minimum number of key reviewers must review this step")]
        MinimumNumber = 2,

       
    }
}

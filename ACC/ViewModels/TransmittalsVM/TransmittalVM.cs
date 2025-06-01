using System.ComponentModel.DataAnnotations;
using DataLayer.Models.Enums;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataLayer.Models;

namespace ACC.ViewModels.TransmittalsVM
{
    public class TransmittalVM
    {
        public int? Id { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Recipient { get; set; }
        public List<int> DocumentVersionIds { get; set; }
        public List<string> Notes { get; set; }
        public List<DocumentVersion> AvailableDocumentVersions { get; set; }
    }

   
}

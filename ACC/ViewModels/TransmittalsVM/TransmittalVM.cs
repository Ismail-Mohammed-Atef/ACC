using System.ComponentModel.DataAnnotations;
using DataLayer.Models.Enums;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ACC.ViewModels.TransmittalsVM
{
    public class TransmittalVM
    {
        public int Id { get; set; }
        public string TransmittalId { get; set; }
        public string Title { get; set; }
        public DateTime SentDate { get; set; }
        public string SenderCompanyName { get; set; }
        public string ProjectName { get; set; }
        public TransmittalStatus Status { get; set; }
        public List<string> RecipientCompanyNames { get; set; }
    }

    public class CreateTransmittalVM
    {
        [Required]
        public string TransmittalId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public int SenderCompanyId { get; set; }
        [Required]
        public List<int> RecipientCompanyIds { get; set; }
        public List<IFormFile> Files { get; set; }
        public string Message { get; set; }
        public List<TransmittalStatus> Statuses { get; set; } = Enum.GetValues(typeof(TransmittalStatus)).Cast<TransmittalStatus>().ToList();
        public List<SelectListItem>? Projects { get; set; }
        public List<SelectListItem>? Companies { get; set; }
    }
}

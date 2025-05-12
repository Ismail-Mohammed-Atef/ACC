using System.ComponentModel.DataAnnotations;
using DataLayer.Models.Enums;
using Microsoft.AspNetCore.Mvc;


namespace ACC.ViewModels
{
    public class CompanyVM
    {
        public int? Id { get; set; }

        [Remote(action:"CheckName",controller:"Company" , ErrorMessage ="Company is already existed.")]
        [Required(ErrorMessage = "Company name is required.")]
        [StringLength(100, ErrorMessage = "Company name cannot exceed 100 characters.")]
        public string? Name { get; set; }

        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string? Address { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [Remote(action: "CheckWebsite", controller: "Company", ErrorMessage = "Website is already existed.")]
        [Url(ErrorMessage = "Please enter a valid website URL.")]
        public string? Website { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\+?\d{10,15}$", ErrorMessage = "Invalid phone number format.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Company trade is required.")]
        public CompanyType? SelectedCompanyType { get; set; }

        public List<CompanyType>? CompanyTypes { get; set; } = Enum.GetValues(typeof(CompanyType)).Cast<CompanyType>().ToList();

        [Required(ErrorMessage = "Country selection is required.")]
        public Country? SelectedCountry { get; set; }

        public List<Country>? Countries { get; set; } = Enum.GetValues<Country>().Cast<Country>().ToList();

    }
}

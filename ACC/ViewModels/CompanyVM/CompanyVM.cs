using System.ComponentModel.DataAnnotations;
using DataLayer.Models.Enums;

namespace ACC.ViewModels
{
    public class CompanyVM
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string? Address { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [Url(ErrorMessage = "Please enter a valid website URL.")]
        public string? Website { get; set; }

        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        [Required(ErrorMessage = "Phone number is required.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Company type is required.")]
        public CompanyType? CompanyType { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        public Country? Country { get; set; }
    }
}

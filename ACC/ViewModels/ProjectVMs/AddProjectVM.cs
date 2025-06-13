using System;
using System.ComponentModel.DataAnnotations;
using DataLayer.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ACC.ViewModels.ProjectVMs
{
    public class AddProjectVM
    {
        [Required(ErrorMessage = "Project name is required.")]
        [StringLength(100, ErrorMessage = "Project name cannot exceed 100 characters.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Project number is required.")]
        [StringLength(50, ErrorMessage = "Project number cannot exceed 50 characters.")]
        public string? ProjectNumber { get; set; }

        [Required(ErrorMessage = "Project type is required.")]
        public ProjectType? ProjectType { get; set; }

        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string? Address { get; set; }


        [Required(ErrorMessage = "Start date is required.")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        [CustomValidation(typeof(AddProjectVM), nameof(ValidateEndDate))]
        public DateTime? EndDate { get; set; }


        public DateTime? CreationDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Project value must be a positive number.")]
        public double? ProjectValue { get; set; }

        [Required(ErrorMessage = "Currency is required.")]
        public Currency? Currency { get; set; }

   

        // Custom validation method for EndDate
        public static ValidationResult? ValidateEndDate(DateTime? endDate, ValidationContext context)
        {
            var instance = context.ObjectInstance as AddProjectVM;
            if (instance == null || !instance.StartDate.HasValue || !endDate.HasValue)
            {
                return ValidationResult.Success;
            }

            if (endDate.Value <= instance.StartDate.Value)
            {
                return new ValidationResult("End date must be greater than the start date.");
            }

            return ValidationResult.Success;
        }
    }
}
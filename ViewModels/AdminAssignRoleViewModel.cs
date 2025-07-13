// File: ViewModels/AdminAssignRoleViewModel.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic; // For SelectListItem, if we choose a dropdown later

namespace GolestanProject.ViewModels
{
    public class AdminAssignRoleViewModel
    {
        [Required(ErrorMessage = "User Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [Display(Name = "User Email to Assign Role")]
        public string TargetUserEmail { get; set; }

        [Required(ErrorMessage = "Please select a role.")]
        [Display(Name = "Select Role")]
        public string SelectedRoleName { get; set; } // Will hold "Student" or "Instructor"

        // Properties for Student Account Details
        [Display(Name = "Student Enrollment Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StudentEnrollmentDate { get; set; } // Nullable, as not always a student

        // Properties for Instructor Account Details
        [Display(Name = "Instructor Salary")]
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive number.")]
        public decimal? InstructorSalary { get; set; } // Nullable, as not always an instructor

        [Display(Name = "Instructor Hire Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? InstructorHireDate { get; set; } // Nullable, as not always an instructor

        // Optionally, if you want a dropdown for roles instead of hardcoded radio buttons,
        // you could add a property like this and populate it in the controller:
        // public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> AvailableRoles { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GolestanProject.Models;

namespace GolestanProject.ViewModels
{
    public class AdminDetailsUserViewModel
    {
        public int Id { get; set; } 

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Primary Role")]
        public string PrimaryRoleName { get; set; }

        public ICollection<Student> Students { get; set; } = new List<Student>();
        public ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();
    }
}
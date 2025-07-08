using System.ComponentModel.DataAnnotations;

namespace GolestanProject.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }          // PRIMARY KEY
        public DateTime created_at { get; set; } = DateTime.Now;
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string hashed_password { get; set; }

        // one to many
        public ICollection<User_Role> USERROLES { get; set; } = new List<User_Role>();
        // one to many
        public ICollection<Instructor> INSTRUCTORS { get; set; } = new List<Instructor>();
        // one to many
        public ICollection<Student> STUDENTS { get; set; } = new List<Student>();
    }
}


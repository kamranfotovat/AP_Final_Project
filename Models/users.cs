using System.ComponentModel.DataAnnotations;

namespace GolestanProject.Models
{
    public class users
    {
        [Key]
        public int id { get; set; }          // PRIMARY KEY
        public DateTime created_at { get; set; } = DateTime.Now;
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string hashed_password { get; set; }

        // one to many
        public ICollection<user_roles> USERROLES { get; set; } = new List<user_roles>();
        // one to many
        public ICollection<instructors> INSTRUCTORS { get; set; } = new List<instructors>();
        // one to many
        public ICollection<students> STUDENTS { get; set; } = new List<students>();
    }
}


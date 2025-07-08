using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GolestanProject.Models
{
    public class Student
    {
        [Key]
        public int student_id { get; set; } //      PRIMARY KEY
        public int user_id { get; set; }
        public DateTime enrollment_date { get; set; }

        [ForeignKey("user_id")]
        public User USER { get; set; }
        public ICollection<Takes> TAKES { get; set; } = new List<Takes>();

    }
}

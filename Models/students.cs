using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GolestanProject.Models
{
    public class students
    {
        [Key]
        public int student_id { get; set; } //      PRIMARY KEY
        public int user_id { get; set; }
        public DateTime enrollment_date { get; set; }

        [ForeignKey("user_id")]
        public users USER { get; set; }
        public ICollection<takes> TAKES { get; set; } = new List<takes>();

    }
}

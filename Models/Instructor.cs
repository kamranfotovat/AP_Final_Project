using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GolestanProject.Models
{
    public class Instructor
    {
        [Key]
        public int instructor_id { get; set; }
        public int user_id { get; set; }
        public decimal salary { get; set; }
        public DateTime hire_date { get; set; }

        [ForeignKey("user_id")]
        public User USER { get; set; }
        public ICollection<Teaches> TEACHES { get; set; } = new List<Teaches>();
    }
}

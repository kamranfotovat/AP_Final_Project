using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GolestanProject.Models
{
    public class instructors
    {
        [Key]
        public int instructor_id { get; set; }
        public int user_id { get; set; }
        public decimal salary { get; set; }
        public DateTime hire_date { get; set; }

        [ForeignKey("user_id")]
        public users USER { get; set; }
        public ICollection<teaches> TEACHES { get; set; } = new List<teaches>();
    }
}

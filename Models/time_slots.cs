using System.ComponentModel.DataAnnotations;

namespace GolestanProject.Models
{
    public class time_slots
    {
        [Key]
        public int id { get; set; }
        public string day { get; set; }
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }

        public ICollection<sections> SECTIONS { get; set; }
    }
}

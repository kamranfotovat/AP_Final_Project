using System.ComponentModel.DataAnnotations;

namespace GolestanProject.Models
{
    public class classrooms
    {
        [Key]
        public int id { get; set; }
        public string building { get; set; }
        public int room_number { get; set; }
        public int capacity { get; set; }


        public ICollection<sections> SECTIONS { get; set; }
    }
}

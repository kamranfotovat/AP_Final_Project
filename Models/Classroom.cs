using System.ComponentModel.DataAnnotations;

namespace GolestanProject.Models
{
    public class Classroom
    {
        [Key]
        public int id { get; set; }
        public string building { get; set; }
        public int room_number { get; set; }
        public int capacity { get; set; }


        public ICollection<Section> SECTIONS { get; set; }
    }
}

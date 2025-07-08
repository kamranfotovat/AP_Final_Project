using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GolestanProject.Models
{
    public class Teaches
    {
        [Key]
        public int id_worthless { get; set; }
        public int instructor_id { get; set; }
        public int section_id { get; set; }


        [ForeignKey("instructor_id")]
        public Instructor INSTRUCTOR { get; set; }
        [ForeignKey("section_id")]
        public Section SECTION { get; set; }
    }
}

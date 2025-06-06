using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GolestanProject.Models
{
    public class teaches
    {
        [Key]
        public int id_worthless { get; set; }
        public int instructor_id { get; set; }
        public int section_id { get; set; }


        [ForeignKey("instructor_id")]
        public instructors INSTRUCTOR { get; set; }
        [ForeignKey("section_id")]
        public sections SECTION { get; set; }
    }
}

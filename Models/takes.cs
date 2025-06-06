using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.AccessControl;

namespace GolestanProject.Models
{
    public class takes
    {
        [Key]
        public int id_worthless { get; set; }
        public int student_id { get; set; }

        public int section_id { get; set; }
        public float grade { get; set; }

        [ForeignKey("student_id")]
        public students STUDENT { get; set; }
        [ForeignKey("section_id")]
        public sections SECTION { get; set; }
    }
}

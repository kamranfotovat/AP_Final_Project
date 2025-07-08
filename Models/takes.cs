using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.AccessControl;

namespace GolestanProject.Models
{
    public class Takes
    {
        [Key]
        public int id_worthless { get; set; }
        public int student_id { get; set; }

        public int section_id { get; set; }
        public float grade { get; set; }

        [ForeignKey("student_id")]
        public Student STUDENT { get; set; }
        [ForeignKey("section_id")]
        public Section SECTION { get; set; }
    }
}

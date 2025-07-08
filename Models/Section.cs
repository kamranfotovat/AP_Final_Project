using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GolestanProject.Models
{
    public class Section
    {
        [Key]
        public int id { get; set; }
        public int course_id { get; set; }
        public int semester { get; set; }
        public int year { get; set; }
        public int classroom_id { get; set; }
        public int time_slot_id { get; set; }


        [ForeignKey("course_id")]
        public Course COURSE { get; set; }
        [ForeignKey("classroom_id")]
        public Classroom CLASSROOMS { get; set; }
        [ForeignKey("time_slot_id")]
        public Time_Slot TIME_SLOT { get; set; }
    }
}

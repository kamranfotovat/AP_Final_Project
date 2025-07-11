﻿using System.ComponentModel.DataAnnotations;

namespace GolestanProject.Models
{
    public class Course
    {
        [Key]
        public int id { get; set; }
        public string title { get; set; }
        public string code { get; set; }
        public string unit { get; set; }
        public string description { get; set; }
        public DateTime final_exam_date { get; set; }

        public Section SECTION { get; set; }
    }
}

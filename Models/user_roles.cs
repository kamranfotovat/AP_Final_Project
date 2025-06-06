using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GolestanProject.Models
{
    public class user_roles
    {
        [Key]
        public int id_worthless { get; set; }
        public int user_id { get; set; }      //        PRIMARY KEY
        public int role_id { get; set; }      //        PRIMARY KEY

        [ForeignKey("user_id")]
        public users USER { get; set; }
        [ForeignKey("role_id")]
        public roles ROLE { get; set; }
    }
}

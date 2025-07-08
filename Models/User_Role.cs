using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GolestanProject.Models
{
    public class User_Role
    {
        [Key]
        public int id_worthless { get; set; }
        public int user_id { get; set; }      //        PRIMARY KEY
        public int role_id { get; set; }      //        PRIMARY KEY

        [ForeignKey("user_id")]
        public User USER { get; set; }
        [ForeignKey("role_id")]
        public Role ROLE { get; set; }
    }
}

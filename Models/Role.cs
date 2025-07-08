using System.ComponentModel.DataAnnotations;

namespace GolestanProject.Models
{
    public class Role
    {
        [Key]
        public int id { get; set; }  //         FOREIGN KEY
        public string name { get; set; }

        public ICollection<User_Role> USERROLES { get; set; } = new List<User_Role>();

    }
}

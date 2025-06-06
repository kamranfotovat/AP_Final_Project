using System.ComponentModel.DataAnnotations;

namespace GolestanProject.Models
{
    public class roles
    {
        [Key]
        public int id { get; set; }  //         FOREIGN KEY
        public string name { get; set; }

        public ICollection<user_roles> USERROLES { get; set; } = new List<user_roles>();

    }
}

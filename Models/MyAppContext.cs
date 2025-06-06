using Microsoft.EntityFrameworkCore;
using GolestanProject.Models;

namespace GolestanProject.Models
{
    public class MyAppContext : DbContext
    {
        public MyAppContext(DbContextOptions<MyAppContext> options) : base(options) { }

        //public DbSet<Item> Items { get; set; }
        public DbSet<users> Users { get; set; }
        public DbSet<roles> Roles { get; set; }
        public DbSet<user_roles> User_Roles  { get; set; }
        public DbSet<courses> Courses { get; set; }
        public DbSet<sections> Sections { get; set; }
        public DbSet<classrooms> Classrooms { get; set; }
        public DbSet<instructors> Instructors { get; set; }
        public DbSet<students> Students { get; set; }
        public DbSet<teaches> Teaches { get; set; }
        public DbSet<takes> Takes { get; set; }
        public DbSet<time_slots> Time_Slots { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

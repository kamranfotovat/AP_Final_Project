using Microsoft.EntityFrameworkCore;

namespace GolestanProject.Models
{
    public class MyAppContext : DbContext
    {
        public MyAppContext(DbContextOptions<MyAppContext> options) : base(options) { }

        public DbSet<Item> Items { get; set; }
    }
}

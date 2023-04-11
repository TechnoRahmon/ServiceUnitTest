using Microsoft.EntityFrameworkCore;

namespace Api.Models
{
    public class DBContextModel : DbContext
    {
        public DBContextModel(DbContextOptions<DBContextModel> options):base(options)
        {
        }
        public DbSet<Order> Orders { get; set; }
        // Add other DbSets for your other entities here
    }
}

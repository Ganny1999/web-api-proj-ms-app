using Microsoft.EntityFrameworkCore;
using order_api_ms.DomainModel.Models;

namespace order_api_ms.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItems> CartItems { get; set; }
    }
}
 
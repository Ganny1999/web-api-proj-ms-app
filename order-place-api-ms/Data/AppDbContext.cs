using Microsoft.EntityFrameworkCore;
using order_place_api_ms.DomainModel.Models;

namespace order_place_api_ms.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
        {            
        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
    }
}

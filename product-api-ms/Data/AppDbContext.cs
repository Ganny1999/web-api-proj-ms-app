using Microsoft.EntityFrameworkCore;
using product_api_ms.DomainModel.Models;

namespace product_api_ms.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
    }
}
using Microsoft.EntityFrameworkCore;
using rating_api_ms.DomainModel.Models;

namespace rating_api_ms.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
        }

        public DbSet<Rating> Ratings { get; set; }
        public DbSet<AverageProductRating> AverageProductRatings { get; set; }

        // Map the view to the class
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AverageProductRating>().ToView("vw_Rating_Average").HasKey(s => s.ProductID);

            base.OnModelCreating(modelBuilder);
        }
    }
}

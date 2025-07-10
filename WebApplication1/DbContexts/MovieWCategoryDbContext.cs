using Microsoft.EntityFrameworkCore;
using WebApplication1.Entitites;

namespace WebApplication1.DbContexts
{
    public class MovieWCategoryDbContext : DbContext
    {
        public MovieWCategoryDbContext(DbContextOptions<MovieWCategoryDbContext> options)  : base(options)
        {
        }
        public DbSet<MovieCategory> MovieCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieCategory>()
                .HasKey(mc => new { mc.MovieId, mc.CategoryId });

            modelBuilder.Entity<MovieCategory>()
                .HasOne(mc => mc.Movie)
                .WithMany(m => m.MovieCategories)
                .HasForeignKey(mc => mc.MovieId);

            modelBuilder.Entity<MovieCategory>()
                .HasOne(mc => mc.Category)
                .WithMany(c => c.MovieCategories)
                .HasForeignKey(mc => mc.CategoryId);
        }

    }
}

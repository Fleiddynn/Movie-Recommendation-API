using Microsoft.EntityFrameworkCore;
using WebApplication1.Entitites;

namespace WebApplication1.MovieRecData 
{
    public class MDbContext : DbContext
    {
        public MDbContext(DbContextOptions<MDbContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.Property(e => e.Cast)
                      .HasColumnType("jsonb");

                entity.Property(e => e.Categories)
                      .HasColumnType("jsonb");

                entity.Property(e => e.Title).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Director).HasMaxLength(255);
            });
        }
    }
}
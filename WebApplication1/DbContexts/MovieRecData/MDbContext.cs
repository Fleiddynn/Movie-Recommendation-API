using Microsoft.EntityFrameworkCore;
using WebApplication1.Entitites;

namespace WebApplication1.DbContexts.MovieRecData 
{
    public class MDbContext : DbContext
    {
        public MDbContext(DbContextOptions<MDbContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; } = null!;
        public DbSet<MovieCategory> MovieCategories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.Property(e => e.Title).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Director).HasMaxLength(255).IsRequired();
                entity.Property(e => e.IMDB).HasColumnType("decimal(3, 1)");
                entity.Property(e => e.Length).IsRequired();
                entity.Property(e => e.ReleaseDate).HasColumnType("date");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            modelBuilder.Entity<MovieCategory>()
                .HasKey(mc => new { mc.MovieId, mc.CategoryId });

        }
    }
}
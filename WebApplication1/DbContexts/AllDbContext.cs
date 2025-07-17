using Microsoft.EntityFrameworkCore;
using WebApplication1.Entitites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebApplication1.DbContexts
{
    public class AllDbContext : IdentityDbContext<User>
    {
        public AllDbContext(DbContextOptions<AllDbContext> options)
            : base(options)
        {
        }
        public DbSet<Movie> Movies { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<MovieCategory> MovieCategories { get; set; } = null!;
        public DbSet<UserReview> UserReviews { get; set; } = null!;
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

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(255).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

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
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.social_login_provider).HasDefaultValue(string.Empty);
                entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            modelBuilder.Entity<UserReview>(entity =>
            {
                entity.Property(e => e.Note);
                entity.Property(e => e.Rating).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            modelBuilder.Entity<UserReview>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserReviews)
                .HasForeignKey(ur => ur.UserId);
            modelBuilder.Entity<UserReview>()
                .HasOne(ur => ur.Movie)
                .WithMany(m => m.UserReviews)
                .HasForeignKey(ur => ur.MovieId);
        }
    }
}
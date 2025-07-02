using Microsoft.EntityFrameworkCore;
using WebApplication1.Entitites;

namespace WebApplication1.UserData
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.first_name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.last_name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.email).HasMaxLength(255).IsRequired();
                entity.Property(e => e.password).HasMaxLength(255).IsRequired();
                entity.Property(e => e.social_login_provider).HasMaxLength(50);
            });
        }
    }
}

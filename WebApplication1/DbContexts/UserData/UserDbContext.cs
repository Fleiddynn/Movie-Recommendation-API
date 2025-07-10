using Microsoft.EntityFrameworkCore;
using WebApplication1.Entitites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebApplication1.DbContexts.UserData
{
    public class UserDbContext : IdentityDbContext<User>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.first_name).IsRequired();
                entity.Property(e => e.last_name).IsRequired();
                entity.Property(e => e.email).IsRequired();
                entity.Property(e => e.password).IsRequired();
                entity.Property(e => e.social_login_provider).HasDefaultValue(string.Empty);
                entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }

    }
}

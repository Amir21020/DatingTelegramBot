using DatingTelegramBot.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace DatingTelegramBot.Persistence.Data
{
    public class DatingDbContext : DbContext
    {
        public DatingDbContext(DbContextOptions<DatingDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>()
                .OwnsOne(u => u.Coordinates);
            modelBuilder.Entity<UserEntity>()
                .HasKey(x => x.UserEntityId);
        }

        public DbSet<UserEntity> Users { get; set; }
    }
}

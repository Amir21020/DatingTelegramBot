using DatingTelegramBot.Application.Repository.Abstraction;
using DatingTelegramBot.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace DatingTelegramBot.Persistence.Data;

public sealed class DatingDbContext(DbContextOptions<DatingDbContext> options) : DbContext(options), IDatingDbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>()
            .OwnsOne(u => u.Coordinates);
        modelBuilder.Entity<UserEntity>()
            .HasKey(x => x.UserEntityId);
    }

    public async Task SaveChangesAsync()
    {
        await base.SaveChangesAsync();
    }

    public DbSet<UserEntity> Users { get; set; }
}

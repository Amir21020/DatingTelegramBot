using DatingTelegramBot.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace DatingTelegramBot.Application.Repository.Abstraction;

public interface IDatingDbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public Task SaveChangesAsync();
}

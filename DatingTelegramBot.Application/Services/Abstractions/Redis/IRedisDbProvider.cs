using StackExchange.Redis;

namespace DatingTelegramBot.Application.Services.Abstractions.Redis;

public interface IRedisDbProvider : IDisposable
{
    public IDatabase Database { get; }
}
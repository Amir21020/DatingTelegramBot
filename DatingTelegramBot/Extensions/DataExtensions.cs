using DatingTelegramBot.Application.Repository.Abstraction;
using DatingTelegramBot.Application.Repository.Commands;
using DatingTelegramBot.Application.Services.Abstractions.Redis;
using DatingTelegramBot.Persistence.Data;
using DatingTelegramBot.Service.Redis;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace DatingTelegramBot.Extensions;

public static class DataExtensions
{
    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<DatingDbContext>(options =>
        options.UseNpgsql(config.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly(typeof(DatingDbContext).Assembly.FullName)));
        services.AddScoped<IDatingDbContext>(provider => provider.GetService<DatingDbContext>());
        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssembly(typeof(CreateUserCommand).Assembly);
        });
        services.AddSingleton<IRedisDbProvider>(provider =>
        {
            var redisConnectionString = config.GetConnectionString("Redis");
            return new RedisDbProvider(redisConnectionString);
        });

        services.AddSingleton<IConnectionMultiplexer>(option =>
                ConnectionMultiplexer.Connect(new ConfigurationOptions
                {
                    EndPoints = { "localhost:6379" },
                     AbortOnConnectFail = false,
                }));
        return services;
    }
}

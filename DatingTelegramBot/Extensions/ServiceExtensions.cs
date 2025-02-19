using DatingTelegramBot.Application.Helpers;
using DatingTelegramBot.Application.Repository.Options;
using DatingTelegramBot.Application.Services.Abstractions.Builder;
using DatingTelegramBot.Application.Services.Abstractions.Commands;
using DatingTelegramBot.Application.Services.Abstractions.Telegram;
using DatingTelegramBot.Domain.Abstractions;
using DatingTelegramBot.Domain.Entity;
using DatingTelegramBot.Domain.Enum;
using DatingTelegramBot.Options;
using DatingTelegramBot.Service.Helpers;
using DatingTelegramBot.Service.Options;
using DatingTelegramBot.Service.Services.Builder;
using DatingTelegramBot.Service.Services.Commands;
using DatingTelegramBot.Service.Services.Telegram.Commands;
using DatingTelegramBot.Service.Services.Telegram.Commands.Base;
using DatingTelegramBot.Service.Services.Telegram.Commands.LanguageCommand;
using DatingTelegramBot.Service.Services.Telegram.Commands.Profile;
using DatingTelegramBot.Service.Services.Telegram.Commands.Profile.Registrations;
using DatingTelegramBot.Service.Services.Telegram.Commands.Profile.Update;
using DatingTelegramBot.Service.Services.Telegram.Commands.Profile.Updates;
using DatingTelegramBot.Service.TelegramBot;

namespace DatingTelegramBot.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddTelegramBot(this IServiceCollection service)
    {
        AddTelegramCommand(service);
        AddProfileService(service);
        AddLanguageService(service);
        service.AddSingleton<IBot, Bot>();
        return service;
    }

    public static IServiceCollection AddOptions(this IServiceCollection service, IConfiguration config)
    {
        service.Configure<MapOptions>(config.GetSection(nameof(MapOptions)));
        service.Configure<DirectoryOptions>(config.GetSection(nameof(DirectoryOptions)));
        service.Configure<CachePrefixOptions>(config.GetSection(nameof(CachePrefixOptions)));
        service.Configure<TokenOptions>(config.GetSection(nameof(TokenOptions)));
        return service;
    }
    public static IServiceCollection AddHelpers(this IServiceCollection service)
    {
        service.AddScoped<IMapLocationService, YandexLocationServiceWrapper>();
        service.AddScoped<IButtonCommandHelper, ButtonCommandHelpers>();
        service.AddScoped<IPhotoHelper, PhotoHelper>();
        return service;
    }
    private static void AddTelegramCommand(this IServiceCollection service)
    {
        service.AddSingleton<ICommandExecutor, CommandExecutor>();
        service.AddScoped<TelegramCommand, DisplayProfileDetailsAndOptionsCommand>();
        service.AddScoped<TelegramCommand, ViewUserProfileCommand>();
        service.AddScoped<TelegramCommand, HandleLikeCommand>();
        service.AddScoped<TelegramCommand, HandleUpdateMessageDescriptionCommand>();
        service.AddScoped<TelegramCommand, HandleUpdatePhotoCommand>();
        service.AddScoped<TelegramCommand, HandleUpdateMessageDescriptionCommand>();
        service.AddScoped<TelegramCommand, HandleUpdatePhotoCommand>();
        service.AddScoped<TelegramCommand, SendMessageDescriptionCommand>();
        service.AddScoped<TelegramCommand, SendUpdatePhotoCommand>();
        service.AddScoped<TelegramCommand, HandleLanguageCommand>();
        service.AddScoped<TelegramCommand, HandleAgeCommand>();
        service.AddScoped<TelegramCommand, HandleCoordinateCommand>();
        service.AddScoped<TelegramCommand, HandleDescriptionCommand>();
        service.AddScoped<TelegramCommand, HandleGenderCommand>();
        service.AddScoped<TelegramCommand, HandleInteresGenderCommand>();
        service.AddScoped<TelegramCommand, HandleSendPhotoCommand>();
        service.AddScoped<TelegramCommand, HandleUserNameCommand>();
        service.AddScoped<TelegramCommand, SendAgeCommand>();
        AddMessageBuilder(service);
    }


    private static void AddProfileService(this IServiceCollection service)
    {
        service.AddScoped<IStickerCommandService, StickerCommandService>();
        service.AddScoped<IUpdateCommandService<Photo>, UpdatePhotoCommandService>();
        service.AddScoped<IUpdateCommandService<Description>,  UpdateDescriptionCommandService>();
        service.AddScoped<ICommandService<Age>, AgeCommandService>();
        service.AddScoped<ICommandService<Description>, DescriptionCommandService>();
        service.AddScoped<ICommandService<Photo>, PhotoCommandService>();
        service.AddScoped<ICommandService<Coordinates>, CoordinateCommandService>();
        service.AddScoped<ICommandService<Gender>, GenderCommandService>();
        service.AddScoped<ICommandService<GenderSearch>, SearchInterestCommandService>();
        service.AddScoped<ICommandService<UserName>, UserNameCommandService>();
        service.AddScoped<IProfileViewCommandService, ProfileViewCommandService>();
    }

    private static void AddLanguageService(this IServiceCollection service)
    {
        service.AddScoped<ICommandService<Language>, LanguageCommandService>();
        service.AddScoped<TelegramCommand, LanguageCommand>();
    }

    private static void AddMessageBuilder(this IServiceCollection service)
    {
        service.AddScoped<IMessageBuilder, AgeRequestBuilder>();
        service.AddScoped<IAgeRequestBuilder, AgeRequestBuilder>();
        service.AddScoped<IMessageBuilder, DescriptionRequestBuilder>();
        service.AddScoped<IDescriptionRequestBuilder, DescriptionRequestBuilder>();
        service.AddScoped<IMessageBuilder, LanguageRequestBuilder>();
        service.AddScoped<ILanguageRequestBuilder, LanguageRequestBuilder>();
        service.AddScoped<IMessageBuilder, LocationRequestBuilder>();
        service.AddScoped<ILocationRequestBuilder, LocationRequestBuilder>();
        service.AddScoped<IMessageBuilder, PhotoRequestBuilder>();
        service.AddScoped<IPhotoRequestBuilder, PhotoRequestBuilder>();
        service.AddScoped<IMessageBuilder, MediaRequestBuilder>();
        service.AddScoped<IMediaRequestBuilder, MediaRequestBuilder>();
        service.AddScoped<IMessageBuilder, ProfileRequestBuilder>();
        service.AddScoped<IProfileRequestBuilder, ProfileRequestBuilder>();
    }
}

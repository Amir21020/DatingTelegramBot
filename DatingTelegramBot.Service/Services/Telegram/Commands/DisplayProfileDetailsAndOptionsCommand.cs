using DatingTelegramBot.Application.Services.Abstractions.Builder;
using DatingTelegramBot.Application.Services.Abstractions.Commands;
using DatingTelegramBot.Service.Services.Telegram.Commands.Base;
using MediatR;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace DatingTelegramBot.Service.Services.Telegram.Commands;

public sealed class DisplayProfileDetailsAndOptionsCommand(IMediator mediator,
    IProfileViewCommandService profileViewCommandService,
    IProfileRequestBuilder profileRequestBuilder,
    ILogger<DisplayProfileDetailsAndOptionsCommand> logger) : TelegramCommand(mediator)
{
    public override string Name => "display_profile_details_and_options";

    public override async Task ExecuteAsync(Update update, string lng)
    {
        logger.LogInformation("Executing command: {CommandName} for chat ID: {ChatId}",
            Name, update.Message.Chat.Id);
        await profileViewCommandService.HandleUserProfileRequestAsync(update, lng);
        await profileRequestBuilder.SetShowProfileOptions(update.Message.Chat.Id, lng).SendAsync();
        logger.LogInformation("Successfully executed command: {CommandName} for chat ID: {ChatId}",
               Name, update.Message.Chat.Id);
    }
}

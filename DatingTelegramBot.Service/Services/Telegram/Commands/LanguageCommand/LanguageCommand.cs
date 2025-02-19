using MediatR;
using DatingTelegramBot.Service.Services.Telegram.Commands.Base;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using DatingTelegramBot.Application.Services.Abstractions.Commands;
using DatingTelegramBot.Domain.Enum;

namespace DatingTelegramBot.Service.Services.Telegram.Commands.LanguageCommand;

public sealed class LanguageCommand(ILogger<LanguageCommand> logger,
    ICommandService<Language> lngCommandService,IMediator mediatR) : TelegramCommand(mediatR)
{
    public override string Name { get; } = "/language";

    public override async Task ExecuteAsync(Update update, string lng)
    {
        logger.LogInformation("Received language change command for ChatId: {ChatId}, Requested Language: {Language}", update.Message!.Chat.Id, lng);
        await lngCommandService.SendMessageAsync(update, lng);
        logger.LogInformation("Language change processed successfully for ChatId: {ChatId} with Language: {Language}", update.Message.Chat.Id, lng);
    }
}

using DatingTelegramBot.Application.Repository.Commands;
using DatingTelegramBot.Application.Repository.Query;
using DatingTelegramBot.Application.Services.Abstractions.Builder;
using DatingTelegramBot.Application.Services.Abstractions.Commands;
using DatingTelegramBot.Domain.Enum;
using DatingTelegramBot.Service.Services.Commands;
using DatingTelegramBot.Service.Services.Telegram.Commands.Base;
using MediatR;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace DatingTelegramBot.Service.Services.Telegram.Commands.LanguageCommand;

public sealed class HandleLanguageCommand(IMediator mediator, ICommandService<Language> lngCommandService, IProfileViewCommandService profileViewCommandService,
    IProfileRequestBuilder profileRequestBuilder,
    ICommandService<Age> ageCommandService,
    ILogger<HandleLanguageCommand> logger) : TelegramCommand(mediator)
{
    public override string Name => "set_language";

    public async override Task ExecuteAsync(Update update, string lng)
    {
        var chatId = update.Message!.Chat.Id;
        logger.LogInformation("Processing language change for ChatId: {ChatId}, Requested Language: {Language}", chatId, lng);

        var result = await lngCommandService.RetrieveMessageAsync(update, lng);

        if (result._error is not null)
        {
            logger.LogWarning("Invalid language request for ChatId: {ChatId}, Requested Language: {Language}. Error: {ErrorMessage}", chatId, lng, result._error.Message);
            await lngCommandService.SendInvalidMessageAsync(update, lng);
        }
        else
        {
            logger.LogInformation("Language successfully set for ChatId: {ChatId}, New Language: {Language}", chatId, result._value);
            await mediatR.Send(new SetLanguageCommand(chatId, result._value));
            var user = await mediatR.Send(new GetUserByIdQuery(chatId));
            if (user._value is not null)
            {
                await profileViewCommandService.HandleUserProfileRequestAsync(update, result._value.ToString());
                await profileRequestBuilder.SetShowProfileOptions(update.Message.Chat.Id, result._value.ToString()).SendAsync();
            }
            else
                await ageCommandService.SendMessageAsync(update, result._value.ToString());
        }
    }
}

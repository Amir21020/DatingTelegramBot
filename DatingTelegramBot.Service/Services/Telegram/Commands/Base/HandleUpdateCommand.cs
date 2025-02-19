using DatingTelegramBot.Application.Repository.Query;
using DatingTelegramBot.Application.Services.Abstractions.Builder;
using DatingTelegramBot.Application.Services.Abstractions.Commands;
using DatingTelegramBot.Domain.DTO.Responses;
using DatingTelegramBot.Domain.Errors;
using MediatR;

namespace DatingTelegramBot.Service.Services.Telegram.Commands.Base;
public abstract class HandleUpdateCommand<T>(
    IMediator mediator,
    IUpdateCommandService<T> updateCommandService,
    IProfileViewCommandService profileViewCommandService,
    IProfileRequestBuilder profileRequestBuilder
) : TelegramCommand(mediator)
{
    public override async Task ExecuteAsync(global::Telegram.Bot.Types.Update update, string lng)
    {
        var result = await updateCommandService.RetrieveMessageAsync(update, lng);
        var user = await mediatR.Send(new GetUserByIdQuery(update.Message.Chat.Id));

        if (result._error is not null && !IsErrorAllowed(result._error))
        {
            await updateCommandService.SendInvalidMessageAsync(update, lng);
            return;
        }

        if (result._value is not null)
        {
            await UpdateUserCommandAsync(user._value, result._value);
        }

        await profileViewCommandService.HandleUserProfileRequestAsync(update, lng);
        await profileRequestBuilder.SetShowProfileOptions(update.Message.Chat.Id, lng).SendAsync();
    }

    protected abstract bool IsErrorAllowed(Error error);

    protected abstract Task UpdateUserCommandAsync(UserResponse user, T value);
}

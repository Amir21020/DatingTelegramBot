using DatingTelegramBot.Application.Repository.Commands;
using DatingTelegramBot.Application.Services.Abstractions.Builder;
using DatingTelegramBot.Application.Services.Abstractions.Commands;
using DatingTelegramBot.Domain.DTO.Responses;
using DatingTelegramBot.Domain.Errors;
using DatingTelegramBot.Domain.Errors.Service.User;
using DatingTelegramBot.Service.Services.Commands;
using DatingTelegramBot.Service.Services.Telegram.Commands.Base;
using MediatR;

namespace DatingTelegramBot.Service.Services.Telegram.Commands.Profile.Update;

public sealed class HandleUpdateMessageDescriptionCommand : HandleUpdateCommand<Description>
{
    public HandleUpdateMessageDescriptionCommand(IMediator mediator,
                                                 IUpdateCommandService<Description> updateDescriptionCommandService,
                                                 IProfileViewCommandService profileViewCommandService,
                                                 IProfileRequestBuilder profileRequestBuilder)
        : base(mediator, updateDescriptionCommandService, profileViewCommandService, profileRequestBuilder) { }

    public override string Name => "handle_update_message_description";
    protected override bool IsErrorAllowed(Error error)
    {
        return error == UserUpdateErrors.DescriptionUpdateCancelledError;
    }

    protected override Task UpdateUserCommandAsync(UserResponse user, Description value)
    {
        return mediatR.Send(new UpdateUserCommand(user.ChatId,
                                                    user.UserName,
                                                    user.Age,
                                                    user.ImagePath,
                                                    user.Coordinates,
                                                    user.Gender,
                                                    user.SearchInterest,
                                                    value.ToString()));
    }
}
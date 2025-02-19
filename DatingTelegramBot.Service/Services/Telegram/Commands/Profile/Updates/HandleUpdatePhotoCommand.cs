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

public sealed class HandleUpdatePhotoCommand : HandleUpdateCommand<Photo>
{
    public HandleUpdatePhotoCommand(IMediator mediator,
                                    IUpdateCommandService<Photo> updatePhotoCommandService,
                                    IProfileViewCommandService profileViewCommandService,
                                    IProfileRequestBuilder profileRequestBuilder)
        : base(mediator, updatePhotoCommandService, profileViewCommandService, profileRequestBuilder) { }

    public override string Name => "handle_update_photo";

    protected override bool IsErrorAllowed(Error error) => error == UserUpdateErrors.PhotoUpdateCancelledError;

    protected override Task UpdateUserCommandAsync(UserResponse user, Photo value)
    {
        return mediatR.Send(new UpdateUserCommand(user.ChatId,
                                                   user.UserName,
                                                   user.Age,
                                                   value.ToString(),
                                                   user.Coordinates,
                                                   user.Gender,
                                                   user.SearchInterest,
                                                   user.Description));
    }
}
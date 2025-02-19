using DatingTelegramBot.Application.Repository.Commands;
using DatingTelegramBot.Application.Repository.Query;
using DatingTelegramBot.Application.Services.Abstractions.Builder;
using DatingTelegramBot.Application.Services.Abstractions.Commands;
using DatingTelegramBot.Domain.DTO.Responses;
using DatingTelegramBot.Domain.Errors;
using DatingTelegramBot.Service.Services.Commands;
using DatingTelegramBot.Service.Services.Telegram.Commands.Base;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DatingTelegramBot.Service.Services.Telegram.Commands.Profile.Registrations;
public sealed class HandleSendPhotoCommand(IMediator mediatR,
    ICommandService<Photo> photoCommandService,
    IProfileRequestBuilder profileRequestBuilder,
    IProfileViewCommandService profileViewCommandService,
    ILogger<HandleSendPhotoCommand> logger) : TelegramCommand(mediatR)
{
    public override string Name => "register_handle_send_photo";

    public override async Task ExecuteAsync(global::Telegram.Bot.Types.Update update, string lng)
    {
        long chatId = update.Message.Chat.Id;
        logger.LogInformation("Initiating photo retrieval for chat ID: {ChatId} with language: {Lng}", chatId, lng);

        var photoResult = await photoCommandService.RetrieveMessageAsync(update, lng);

        if (photoResult._error is not null)
        {
            await HandlePhotoRetrievalError(photoResult, update, lng);
            return;
        }

        await ProcessPhotoRetrieval(chatId, photoResult, update, lng);
    }

    private async Task ProcessPhotoRetrieval(long chatId, Result<Photo, Error> photoResult, global::Telegram.Bot.Types.Update update, string lng)
    {
        logger.LogInformation("Successfully retrieved photo: {Photo} for chat ID: {ChatId}", photoResult._value, chatId);

        await AddPhotoPathToHashSet(chatId, photoResult._value.ToString());

        var userFromHashSetResult = await mediatR.Send(new GetUserFromHashSetQuery(chatId));
        await CreateUserProfile(update, userFromHashSetResult);
        await profileViewCommandService.HandleUserProfileRequestAsync(update, lng);
        await profileRequestBuilder.SetShowProfileOptions(update.Message.Chat.Id, lng).SendAsync();
    }

    private async Task HandlePhotoRetrievalError(Result<Photo, Error> photoResult, global::Telegram.Bot.Types.Update update, string lng)
    {
        logger.LogError("Error occurred while retrieving photo for chat ID: {ChatId}. Error details: {@Error}", update.Message.Chat.Id, photoResult._error);
        await photoCommandService.SendInvalidMessageAsync(update, lng);
    }

    private async Task AddPhotoPathToHashSet(long chatId, string photoPath)
    {
        logger.LogInformation("Adding photo path to hash set for chat ID: {ChatId}. Photo path: {PhotoPath}", chatId, photoPath);
        await mediatR.Send(new AddPathPhotoInHashSetCommand(chatId, photoPath));
    }

    private async Task CreateUserProfile(global::Telegram.Bot.Types.Update update, Result<ViewUserResponse, Error> userFromHashSetResult)
    {
        if (userFromHashSetResult._value is null)
        {
            logger.LogWarning("No user found for chat ID: {ChatId}", update.Message.Chat.Id);
            return ;
        }

        var user = userFromHashSetResult._value;

        logger.LogInformation("Creating user profile for chat ID: {ChatId} for user: {UserName}", update.Message.Chat.Id, user.UserName);

        await mediatR.Send(new CreateUserCommand(
            update.Message.Chat.Id,
            user.UserName,
            user.Age,
            user.ImagePath,
            user.Coordinates,
            user.Gender,
            update.Message.Chat.Username,
            user.InterestGender,
            user.Description)
        );
    }
}

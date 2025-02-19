using DatingTelegramBot.Application.Helpers;
using DatingTelegramBot.Application.Services.Abstractions.Builder;
using DatingTelegramBot.Application.Services.Abstractions.Commands;
using DatingTelegramBot.Domain.Entity;
using DatingTelegramBot.Domain.Errors;
using DatingTelegramBot.Domain.Errors.Service.User;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DatingTelegramBot.Service.Services.Commands;

public sealed class CoordinateCommandService(IMapLocationService mapLocationService,
    ILocationRequestBuilder locationRequestBuilder,
    ILogger<CoordinateCommandService> logger) : ICommandService<Coordinates>
{
    public async Task<Result<Coordinates, Error>> RetrieveMessageAsync(Update update, string lng)
    {
        var chatId = update.Message.Chat.Id;

        logger.LogInformation("Received message from ChatId: {ChatId}, MessageType: {MessageType}", chatId, update.Message.Type);

        if (update.Message.Type == MessageType.Location)
        {
            var coordinates = new Coordinates(update.Message.Location.Latitude, update.Message.Location.Longitude);
            logger.LogInformation("Coordinates received from location: {Coordinates}", coordinates);
            return coordinates;
        }
        else if (update.Message.Type == MessageType.Text)
        {
            logger.LogInformation("Processing address input: {Address}", update.Message.Text);
            return await mapLocationService.GetLatLongFromAddressAsync(update.Message.Text);
        }

        logger.LogWarning("Invalid message type for coordinates from ChatId: {ChatId}", chatId);
        return UserRegistrationErrors.CoordinatesFormatError;
    }

    public async Task SendInvalidMessageAsync(Update update, string lng)
    {
        logger.LogWarning("Sending invalid data error message to ChatId: {ChatId}, Language: {Language}", update.Message.Chat.Id, lng);
        await locationRequestBuilder.SetInvalidDataError(update.Message.Chat.Id, lng).SendAsync();
        logger.LogInformation("Invalid data error message sent successfully to ChatId: {ChatId}", update.Message.Chat.Id);
    }

    public async Task SendMessageAsync(Update update, string lng)
    {
        var chatId = update.Message.Chat.Id;
        logger.LogInformation("Sending location request message to ChatId: {ChatId}, Language: {Language}", chatId, lng);
        await locationRequestBuilder.SetLocationRequest(chatId, lng).SendAsync();
        logger.LogInformation("Location request message sent successfully to ChatId: {ChatId}", chatId);
    }
}

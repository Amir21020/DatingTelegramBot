using DatingTelegramBot.Domain.Errors;
using Telegram.Bot.Types;

namespace DatingTelegramBot.Application.Helpers;

public interface IPhotoHelper
{
    Task SendPhotoAsync(long chatId, string filePath, string question = null, string[] options = null);
    Task DownloadPhotoAsync(Update update);
    Result<string, Error> GetPathPhoto(Update update);
}

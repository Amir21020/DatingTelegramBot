using DatingTelegramBot.Application.Helpers;
using DatingTelegramBot.Domain.Abstractions;
using DatingTelegramBot.Domain.Errors;
using DatingTelegramBot.Domain.Errors.Helpers;
using DatingTelegramBot.Service.Helpers.BaseClass;
using DatingTelegramBot.Service.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace DatingTelegramBot.Service.Helpers;

public sealed class PhotoHelper(IOptions<DirectoryOptions> baseDirectory,
    IBot bot,
    IButtonCommandHelper buttonCommandHelper,
    ILogger<BasePhotoHelper> logger) : BasePhotoHelper(baseDirectory, logger), IPhotoHelper
{
    private readonly TelegramBotClient _bot = bot.GetTelegramBot().Result;
    public async Task DownloadPhotoAsync(Update update)
    {
        var (fileId, chatId) = GetPhotoDetails(update);
        var file = await _bot.GetFileAsync(fileId);
        await EnsureDirectoryExistsAsync(chatId);

        var filePath = GetFilePath(chatId, fileId);
        logger.LogInformation("Downloading photo with FileId={FileId} to {FilePath}", fileId, filePath);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await _bot.DownloadFile(file.FilePath, fileStream);
            logger.LogInformation("Photo downloaded successfully: {FilePath}", filePath);
        }
    }

    public async Task SendPhotoAsync(long chatId, string filePath, string question = null, string[] options = null)
    {
        logger.LogInformation("Sending photo from {FilePath} to ChatId={ChatId}", filePath, chatId);

        using (var fileStream = new FileStream(filePath, FileMode.Open))
        {
            var inputFile = new InputFileStream(fileStream, Path.GetFileName(filePath));
            await _bot.SendPhotoAsync(chatId: chatId, photo: inputFile, caption: question, replyMarkup: options == null ? null : buttonCommandHelper.CreateMainKeyboard(options));
            logger.LogInformation("Photo sent successfully to ChatId={ChatId}", chatId);
        }
    }

    public Result<string, Error> GetPathPhoto(Update update)
    {
        var (fileId, chatId) = GetPhotoDetails(update);
        var filePath = GetFilePath(chatId, fileId);

        if (System.IO.File.Exists(filePath))
        {
            logger.LogInformation("File found at {FilePath}", filePath);
            return filePath;
        }
        else
        {
            logger.LogWarning("File path not found for FileId={FileId}, ChatId={ChatId}", fileId, chatId);
            return PhotoErrors.FilePathNotFoundError;
        }
    }
}

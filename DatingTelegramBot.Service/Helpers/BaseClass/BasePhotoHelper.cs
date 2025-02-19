using DatingTelegramBot.Service.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;

namespace DatingTelegramBot.Service.Helpers.BaseClass;

public abstract class BasePhotoHelper(IOptions<DirectoryOptions> baseDirectory,
    ILogger<BasePhotoHelper> logger)
{
    protected readonly DirectoryOptions _baseDirectory = baseDirectory.Value;
    protected string GetChatDirectory(long chatId)
    {
        return Path.Combine(_baseDirectory.BaseDirectoryPath, chatId.ToString());
    }

    protected string GetFilePath(long chatId, string fileId)
    {
        return Path.Combine(GetChatDirectory(chatId), $"{fileId}.jpg");
    }

    protected (string fileId, long chatId) GetPhotoDetails(Update update)
    {
        var largestPhoto = update.Message.Photo.OrderByDescending(p => p.FileSize).FirstOrDefault();
        var fileId = largestPhoto?.FileId;
        var chatId = update.Message.Chat.Id;

        logger.LogInformation("Retrieved photo details: FileId={FileId}, ChatId={ChatId}", fileId, chatId);
        return (fileId, chatId);
    }

    protected async Task EnsureDirectoryExistsAsync(long chatId)
    {
        var chatDir = GetChatDirectory(chatId);
        if (!Directory.Exists(chatDir))
        {
            logger.LogInformation("Creating directory for ChatId={ChatId} at path={ChatDir}", chatId, chatDir);
            Directory.CreateDirectory(chatDir);
            logger.LogInformation("Directory created successfully at path={ChatDir}", chatDir);
        }
        else
        {
            logger.LogInformation("Directory already exists for ChatId={ChatId} at path={ChatDir}", chatId, chatDir);
        }
    }
}

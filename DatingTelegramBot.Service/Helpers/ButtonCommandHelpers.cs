using DatingTelegramBot.Application.Helpers;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types.ReplyMarkups;

namespace DatingTelegramBot.Service.Helpers;

public sealed class ButtonCommandHelpers(ILogger<ButtonCommandHelpers> logger) : IButtonCommandHelper
{
    public ReplyKeyboardMarkup CreateMainKeyboard(params string[] buttonLabels)
    {
        logger.LogInformation("Creating main keyboard with buttons: {ButtonLabels}", string.Join(", ", buttonLabels));

        var keyboardButtons = buttonLabels.Select(label => new KeyboardButton(label)).ToArray();

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            keyboardButtons
        })
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = true
        };

        logger.LogInformation("Main keyboard created successfully.");
        return replyKeyboard;
    }
    public InlineKeyboardMarkup CreateInlineKeyboard(params (string label, string url)[] buttons)
    {
        logger.LogInformation("Creating inline keyboard with buttons: {Buttons}", string.Join(", ", buttons.Select(b => $"{b.label} ({b.url})")));

        var inlineKeyboardButtons = buttons
            .Select(button =>
                InlineKeyboardButton.WithUrl(button.label, button.url))
            .ToArray();

        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            inlineKeyboardButtons
        });

        logger.LogInformation("Inline keyboard created successfully.");
        return inlineKeyboard;
    }

    public ReplyKeyboardRemove CreateRemoveButton()
    {
        logger.LogInformation("Creating button to remove the keyboard.");
        return new ReplyKeyboardRemove();
    }

    public ReplyKeyboardMarkup CreateLocationButton(string text)
    {
        logger.LogInformation("Creating button to request location.");

        var locationButton = new KeyboardButton(text)
        {
            RequestLocation = true
        };

        return new ReplyKeyboardMarkup(new[] { locationButton })
        {
            ResizeKeyboard = true
        };
    }
}

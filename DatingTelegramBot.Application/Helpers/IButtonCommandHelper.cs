using Telegram.Bot.Types.ReplyMarkups;

namespace DatingTelegramBot.Application.Helpers;

public interface IButtonCommandHelper
{
    public ReplyKeyboardMarkup CreateMainKeyboard(params string[] buttonLabels);
    public InlineKeyboardMarkup CreateInlineKeyboard(params (string label, string url)[] buttons);
    public ReplyKeyboardRemove CreateRemoveButton();
    public ReplyKeyboardMarkup CreateLocationButton(string text);
}

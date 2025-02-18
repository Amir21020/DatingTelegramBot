using DatingTelegramBot.Application.Services.Abstractions.Telegram;
using DatingTelegramBot.Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using DatingTelegramBot.Service.Services.Telegram.Commands.Base;
using DatingTelegramBot.Application.Repository.Query;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using DatingTelegramBot.Service.Helpers;
using DatingTelegramBot.Application.Services.Abstractions.Builder;

namespace DatingTelegramBot.Service.Services.Telegram.Commands;

public sealed class CommandExecutor(IServiceScopeFactory serviceProvider,
    IBot bot, ILogger<CommandExecutor> logger) : ICommandExecutor
{
    private readonly List<TelegramCommand> _commands = serviceProvider
        .CreateScope()
        .ServiceProvider
        .GetServices<TelegramCommand>()
        .ToList();

    private readonly IProfileRequestBuilder profileRequestBuilder
        = serviceProvider
        .CreateScope()
        .ServiceProvider
        .GetRequiredService<IProfileRequestBuilder>();
    private readonly IMediator mediatR = serviceProvider
        .CreateScope()
        .ServiceProvider
        .GetRequiredService<IMediator>();
    private readonly TelegramBotClient _bot = bot.GetTelegramBot().Result;
    private readonly UserCommandState userCommandState = new();

    public async Task ExecuteAsync(Update update)
    {
        if (update?.Message?.Chat == null)
        {
            logger.LogWarning("Received update with no valid chat information.");
            return;
        }
        if (update.Id <= userCommandState.lastUpdateId)
            return;

        var typeMessage = update.Message.Type;
        var text = update.Message.Text;
        var chatId = update.Message.Chat.Id;

        logger.LogInformation("Processing update for chat ID: {ChatId} with text: {Text}", chatId, text);

        var lng = await mediatR.Send(new GetLanguageByIdFromCacheQuery(chatId));
        var user = await mediatR.Send(new GetUserByIdQuery(chatId));
        var resultLastMessage = await mediatR.Send(new GetLastMessageTgBotInCacheQuery(update.Message.Chat.Id));
        userCommandState.lastTextMessage = resultLastMessage._value;
        switch (text)
        {
            case "/language":
                await ExecuteCommandAsync("/language", update, lng._value != null ? lng._value.ToString() : "ru");
                return;
            case "/myprofile":
            case "/start":
                if (lng._error is not null)
                    await ExecuteCommandAsync("/language", update, lng._value != null ? lng._value.ToString() : "ru");
                else if (user._value is not null)
                    await ExecuteCommandAsync("display_profile_details_and_options", update, lng._value.ToString());
                else
                    await ExecuteCommandAsync("register_send_age", update, lng._value.ToString());
                return;
        }

        if (userCommandState.lastTextMessage == await TranslatorCommandHelper.GetTranslationAsync(lng._value.ToString(), "age_request_message"))
        {
            await ExecuteCommandAsync("register_handle_age", update, lng._value.ToString());
            return;
        }
        if (userCommandState.lastTextMessage
            ==
            "1. " + TranslatorCommandHelper.GetTranslationAsync(lng._value.ToString(), "edit_profile_text").Result + "\n"
            + "2. " + TranslatorCommandHelper.GetTranslationAsync(lng._value.ToString(), "fill_questionnaire_again").Result + "\n"
            + "3. " + TranslatorCommandHelper.GetTranslationAsync(lng._value.ToString(), "update_photo").Result + "\n"
            + "4. " + TranslatorCommandHelper.GetTranslationAsync(lng._value.ToString(), "change_profile_text").Result)
        {
            switch (text)
            {
                case "1":
                case "1 🚀":
                    await ExecuteCommandAsync("view_user_profile", update, lng._value.ToString());
                    return;
                case "2":
                    await ExecuteCommandAsync("register_send_age", update, lng._value.ToString());
                    return;
                case "3":
                    await ExecuteCommandAsync("send_update_photo", update, lng._value.ToString());
                    return;
                case "4":
                    await ExecuteCommandAsync("send_update_message_description", update, lng._value.ToString());
                    return;
                default:
                    await profileRequestBuilder.SetInvalidOptionMessage(chatId, lng._value.ToString()).SendAsync();
                    return;
            }
        }
        switch (userCommandState.lastCommand.Name)
        {
            case "/language":
                await ExecuteCommandAsync("set_language", update, lng._value != null ? lng._value.ToString() : "ru");
                return;
            case "set_language":
                if (userCommandState.lastTextMessage == await TranslatorCommandHelper.GetTranslationAsync("ru", "invalid_option_message"))
                    await ExecuteCommandAsync("set_language", update, lng._value != null ? lng._value.ToString() : "ru");
                else if(user._value is not null)
                    await ExecuteCommandAsync("display_profile_details_and_options", update, lng._value.ToString());
                else
                    await ExecuteCommandAsync("register_send_age", update, lng._value.ToString());
                return;
            case "register_send_age":
                await ExecuteCommandAsync("register_handle_age", update, lng._value.ToString());
                return;
            case "register_handle_age":
                if (userCommandState.lastTextMessage == await TranslatorCommandHelper.GetTranslationAsync(lng._value.ToString(), "correct_age"))
                {
                    await ExecuteCommandAsync("register_handle_age", update, lng._value.ToString());
                    return;
                }
                await ExecuteCommandAsync("register_handle_gender", update, lng._value.ToString());
                return;
            case "register_handle_gender":
                if (userCommandState.lastTextMessage == await TranslatorCommandHelper.GetTranslationAsync(lng._value.ToString(), "invalid_option_message"))
                {
                    await ExecuteCommandAsync("register_handle_gender", update, lng._value.ToString());
                    return;
                }
                await ExecuteCommandAsync("register_handle_interes_gender", update, lng._value.ToString());
                return;
            case "register_handle_interes_gender":
                if (userCommandState.lastTextMessage == await TranslatorCommandHelper.GetTranslationAsync(lng._value.ToString(), "invalid_option_message"))
                {
                    await ExecuteCommandAsync("register_handle_interes_gender", update, lng._value.ToString());
                    return;
                }
                await ExecuteCommandAsync("register_handle_coordinate", update, lng._value.ToString());
                return;
            case "register_handle_coordinate":
                if (userCommandState.lastTextMessage == await TranslatorCommandHelper.GetTranslationAsync(lng._value.ToString(), "enter_valid_location"))
                {
                    await ExecuteCommandAsync("register_handle_coordinate", update, lng._value.ToString());
                    return;
                }
                await ExecuteCommandAsync("register_handle_username", update, lng._value.ToString());
                return;
            case "register_handle_username":
                if (userCommandState.lastTextMessage == await TranslatorCommandHelper.GetTranslationAsync(lng._value.ToString(), "enter_valid_name_message"))
                {
                    await ExecuteCommandAsync("register_handle_username", update, lng._value.ToString());
                    return;
                }
                await ExecuteCommandAsync("register_handle_descriprion", update, lng._value.ToString());
                return;
            case "register_handle_descriprion":
                if (userCommandState.lastTextMessage == await TranslatorCommandHelper.GetTranslationAsync(lng._value.ToString(), "enter_valid_location"))
                {
                    await ExecuteCommandAsync("register_handle_descriprion", update, lng._value.ToString());
                    return;
                }
                await ExecuteCommandAsync("register_handle_send_photo", update, lng._value.ToString());
                return;
                case "register_handle_send_photo":
                    if (userCommandState.lastTextMessage == await TranslatorCommandHelper.GetTranslationAsync(lng._value.ToString(), "enter_valid_location"))
                        await ExecuteCommandAsync("register_handle_send_photo", update, lng._value.ToString());
                return;
            case "view_user_profile":
            case "handle_like":
                switch (text)
                {
                    case "❤️":
                        await ExecuteCommandAsync("handle_like", update, lng._value.ToString());
                        return;
                    case "👎":
                        await ExecuteCommandAsync("view_user_profile", update, lng._value.ToString());
                        return;
                    case "💤":
                        await ExecuteCommandAsync("display_profile_details_and_options", update, lng._value.ToString());
                        return;
                    default:
                        await profileRequestBuilder.SetInvalidOptionMessage(chatId, lng._value.ToString()).SendAsync();
                        return;
                }
            case "send_update_photo":
                await ExecuteCommandAsync("handle_update_photo", update, lng._value.ToString());
                return;
            case "send_update_message_description":
                await ExecuteCommandAsync("handle_update_message_description", update, lng._value.ToString());
                return;
            case "handle_update_photo":
                if (TranslatorCommandHelper.GetTranslationAsync(lng._value.ToString(), "enter_valid_location").Result == userCommandState.lastTextMessage)
                {
                    await ExecuteCommandAsync("handle_update_photo", update, lng._value.ToString());
                    return;
                }
                await ExecuteCommandAsync("display_profile_details_and_options", update, lng._value.ToString());
                return;
            case "handle_update_message_description":
                if (TranslatorCommandHelper.GetTranslationAsync(lng._value.ToString(), "enter_valid_location").Result == userCommandState.lastTextMessage)
                {
                    await ExecuteCommandAsync("handle_update_message_description", update, lng._value.ToString());
                    return;
                }
                await ExecuteCommandAsync("display_profile_details_and_options", update, lng._value.ToString());
                return;
        }
    }

    private async Task ExecuteCommandAsync(string commandName, Update update, string lng)
    {
        logger.LogInformation("Executing command: {CommandName} for chat ID: {ChatId}", commandName, update.Message.Chat.Id);

        userCommandState.lastCommand = _commands.First(x => x.Name == commandName);
        userCommandState.lastUpdateId = update.Id;

        await userCommandState.lastCommand.ExecuteAsync(update, lng);
        logger.LogInformation("Completed execution of command: {CommandName} for chat ID: {ChatId}", commandName, update.Message.Chat.Id);
    }
}




public class UserCommandState
{
    public TelegramCommand? lastCommand;
    public long lastUpdateId;
    public string? lastTextMessage;
}
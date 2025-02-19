using MediatR;
using Telegram.Bot.Types;

namespace DatingTelegramBot.Service.Services.Telegram.Commands.Base;

public abstract class TelegramCommand(IMediator mediator)
{
    protected IMediator mediatR => mediator;

    public abstract string Name { get; }
    public abstract Task ExecuteAsync(Update update, string lng);
}

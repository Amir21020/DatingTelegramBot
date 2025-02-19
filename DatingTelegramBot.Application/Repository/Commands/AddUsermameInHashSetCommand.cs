using MediatR;

namespace DatingTelegramBot.Application.Repository.Commands;

public sealed record AddUsermameInHashSetCommand
    (long ChatId, string UserName): IRequest;

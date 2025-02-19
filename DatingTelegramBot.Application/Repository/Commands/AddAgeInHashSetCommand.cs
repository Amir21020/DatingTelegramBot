using MediatR;

namespace DatingTelegramBot.Application.Repository.Commands;

public sealed record AddAgeInHashSetCommand
    (long ChatId, string Age) : IRequest;
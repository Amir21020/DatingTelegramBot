using MediatR;

namespace DatingTelegramBot.Application.Repository.Commands;

public sealed record AddGenderInHashSetCommand(long ChatId, string Gender) : IRequest;
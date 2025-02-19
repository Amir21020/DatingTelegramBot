using MediatR;

namespace DatingTelegramBot.Application.Repository.Commands;

public sealed record AddCoordinatesInHashSetCommand(long ChatId, string Latitude, string Longitude) : IRequest;

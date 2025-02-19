using MediatR;

namespace DatingTelegramBot.Application.Repository.Commands;

public sealed record AddPathPhotoInHashSetCommand(long ChatId, string PhotoPath) : IRequest;

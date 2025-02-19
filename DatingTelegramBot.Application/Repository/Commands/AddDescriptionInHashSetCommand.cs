using MediatR;

namespace DatingTelegramBot.Application.Repository.Commands;

public record class AddDescriptionInHashSetCommand(long ChatId, string Description) : IRequest;

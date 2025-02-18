using DatingTelegramBot.Domain.Enum;
using MediatR;

namespace DatingTelegramBot.Application.Repository.Commands;

public record SetLanguageCommand(long ChatId, Language Language) : IRequest;

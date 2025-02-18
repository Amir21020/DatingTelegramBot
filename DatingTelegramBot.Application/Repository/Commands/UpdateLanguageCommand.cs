using DatingTelegramBot.Domain.Enum;
using MediatR;

namespace DatingTelegramBot.Application.Repository.Commands;

public record UpdateLanguageCommand(long ChatId, Language Lng) : IRequest;

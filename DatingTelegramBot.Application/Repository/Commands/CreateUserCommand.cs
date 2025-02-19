using DatingTelegramBot.Domain.Entity;
using DatingTelegramBot.Domain.Enum;
using MediatR;

namespace DatingTelegramBot.Application.Repository.Commands;

public record CreateUserCommand(long TgChatId, string? UserName,
    int Age, string? ImagePath,
    Coordinates? Coordinates,
    Gender Gender,
    string TgUserName,
    GenderSearch SearchInterest,
    string? Description) : IRequest;

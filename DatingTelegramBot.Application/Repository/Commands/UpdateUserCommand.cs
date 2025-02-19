using DatingTelegramBot.Domain.Entity;

using DatingTelegramBot.Domain.Enum;
using MediatR;

namespace DatingTelegramBot.Application.Repository.Commands;


public record UpdateUserCommand(long ChatId, string? UserName,
    int Age, string? ImagePath,
    Coordinates? Coordinates,
    Gender Gender, GenderSearch SearchInterest,
    string? Description) : IRequest;

using DatingTelegramBot.Domain.Enum;
using MediatR;

namespace DatingTelegramBot.Application.Repository.Commands;

public sealed record AddInteresGenderInHashSetCommand(long ChatId, GenderSearch InteresGender) : IRequest;

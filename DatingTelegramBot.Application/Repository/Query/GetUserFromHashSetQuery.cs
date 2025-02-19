using DatingTelegramBot.Domain.DTO.Responses;
using DatingTelegramBot.Domain.Errors;
using MediatR;

namespace DatingTelegramBot.Application.Repository.Query;

public sealed record GetUserFromHashSetQuery(long ChatId)  : IRequest<Result<ViewUserResponse, Error>>;
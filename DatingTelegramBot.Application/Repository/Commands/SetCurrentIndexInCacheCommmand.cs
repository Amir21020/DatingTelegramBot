using MediatR;

namespace DatingTelegramBot.Application.Repository.Commands;

public record SetCurrentIndexInCacheCommmand(long ChatId, int CurrentIndex) : IRequest;

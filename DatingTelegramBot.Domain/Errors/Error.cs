﻿namespace DatingTelegramBot.Domain.Errors;

public sealed record Error(string Code, string? Message = null);

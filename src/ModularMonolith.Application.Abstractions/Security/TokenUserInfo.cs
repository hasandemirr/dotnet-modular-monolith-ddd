namespace ModularMonolith.Application.Abstractions.Security;

public sealed record TokenUserInfo(
    Guid Id,
    string Email,
    string? Role);
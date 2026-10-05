namespace ModularMonolith.Application.Abstractions.Security;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }
    Guid? Id { get; }
    string? Email { get; }
    string? Role { get; }
}
using ModularMonolith.Application.Abstractions.Security;

namespace ModularMonolith.Application.Abstractions.Directories;

public interface IUserDirectory
{
    Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
    Task<string?> GetEmailAsync(Guid id, CancellationToken ct = default);
    Task<Guid?> GetIdByEmailAsync(string email, CancellationToken ct = default);
    Task<TokenUserInfo?> GetTokenUserInfoAsync(Guid userId, CancellationToken ct = default);
}
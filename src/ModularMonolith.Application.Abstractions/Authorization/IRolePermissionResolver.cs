namespace ModularMonolith.Application.Abstractions.Authorization;

/// <summary>
/// Resolves a role key to its effective permission set from an in-memory snapshot loaded
/// from the DB. GetPermissions is synchronous by design so TokenService.CreateAccessToken
/// stays sync (no per-request DB query). admin is special-cased in code to the full
/// Permissions catalog (fail-safe: never locked out even if the snapshot is empty).
/// </summary>
public interface IRolePermissionResolver
{
    IReadOnlyList<string> GetPermissions(string? roleKey);

    IReadOnlyCollection<string> KnownRoleKeys { get; }

    Task RefreshAsync(CancellationToken ct = default);
}

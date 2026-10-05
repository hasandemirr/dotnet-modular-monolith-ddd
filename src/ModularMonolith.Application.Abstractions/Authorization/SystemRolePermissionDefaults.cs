namespace ModularMonolith.Application.Abstractions.Authorization;

/// <summary>
/// Bootstrap permission sets for the built-in system roles, used ONLY by startup seeding
/// (EnsureRolesAsync). Runtime role->permission resolution is dynamic
/// (IRolePermissionResolver); these are just the initial DB rows for ops/user. admin gets
/// no rows; its permissions come from code (the resolver short-circuits admin).
/// </summary>
public static class SystemRolePermissionDefaults
{
    public static IReadOnlyList<string> Ops { get; } =
    [
        Permissions.Approval.Approve,
    ];

    public static IReadOnlyList<string> User { get; } = [];
}

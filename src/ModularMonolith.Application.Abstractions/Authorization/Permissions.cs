using System.Reflection;

namespace ModularMonolith.Application.Abstractions.Authorization;

public static class Permissions
{
    /// <summary>
    /// Every permission constant declared under this class (reflection over the nested
    /// static classes' public const string fields). Superset of any single role's grants;
    /// used for policy registration and the admin full-catalog short-circuit.
    /// </summary>
    public static IReadOnlyList<string> All { get; } =
        typeof(Permissions)
            .GetNestedTypes(BindingFlags.Public | BindingFlags.Static)
            .SelectMany(t => t.GetFields(
                BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
            .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string))
            .Select(f => (string)f.GetRawConstantValue()!)
            .ToList()
            .AsReadOnly();

    public static class Users
    {
        public const string Manage = "users.manage";
    }

    public static class Approval
    {
        public const string Rules_Manage = "approval.rules.manage";
        public const string Approve = "approval.approve";
    }
}
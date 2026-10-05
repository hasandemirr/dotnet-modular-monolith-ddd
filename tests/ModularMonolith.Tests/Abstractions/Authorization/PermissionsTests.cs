using AwesomeAssertions;
using ModularMonolith.Application.Abstractions.Authorization;

namespace ModularMonolith.Tests.Abstractions.Authorization;

public sealed class PermissionsTests
{
    [Fact]
    public void All_ContainsExactlyDeclaredPermissions()
    {
        Permissions.All.Should().BeEquivalentTo(
            "users.manage",
            "approval.rules.manage",
            "approval.approve");
    }

    [Fact]
    public void All_HasNoDuplicates()
    {
        Permissions.All.Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public void OpsDefaults_AreAllInCatalog()
    {
        SystemRolePermissionDefaults.Ops.Should().BeSubsetOf(Permissions.All);
    }

    [Fact]
    public void UserDefaults_AreAllInCatalog()
    {
        SystemRolePermissionDefaults.User.Should().BeSubsetOf(Permissions.All);
    }
}

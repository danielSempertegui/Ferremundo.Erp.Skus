using Ferremundo.Security.Authentication.Authorization;
using Ferremundo.Security.Authentication.Configuration;
using Ferremundo.Erp.Skus.Api.Tests.TestDoubles;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Ferremundo.Erp.Skus.Api.Tests.Authorization;

[TestClass]
public sealed class PermissionAuthorizationHandlerTests
{
    [TestMethod]
    public async Task HandleAsync_ShouldSucceed_WhenPermissionClaimMatches()
    {
        var requirement = new PermissionRequirement("erp.skus.pricing.read");
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            [new Claim("permission", "erp.skus.pricing.read")],
            authenticationType: "Test"));
        var context = new AuthorizationHandlerContext([requirement], user, resource: null);
        var handler = CreateHandler();

        await handler.HandleAsync(context);

        Assert.IsTrue(context.HasSucceeded);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldNotSucceed_WhenPermissionClaimIsMissing()
    {
        var requirement = new PermissionRequirement("erp.skus.pricing.read");
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            [new Claim("permission", "another.permission")],
            authenticationType: "Test"));
        var context = new AuthorizationHandlerContext([requirement], user, resource: null);
        var handler = CreateHandler();

        await handler.HandleAsync(context);

        Assert.IsFalse(context.HasSucceeded);
    }

    private static PermissionAuthorizationHandler CreateHandler()
        => new(new FixedOptionsMonitor<FerremundoSecurityAuthenticationOptions>(new()
        {
            PermissionClaimType = "permission"
        }));
}

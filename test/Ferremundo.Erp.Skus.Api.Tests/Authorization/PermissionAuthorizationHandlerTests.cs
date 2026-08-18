using Ferremundo.Erp.Skus.Api.Authorization;
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
        var handler = new PermissionAuthorizationHandler();

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
        var handler = new PermissionAuthorizationHandler();

        await handler.HandleAsync(context);

        Assert.IsFalse(context.HasSucceeded);
    }
}

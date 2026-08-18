using Ferremundo.Security.Authentication.Authorization;

namespace Ferremundo.Erp.Skus.Api.Tests.Authorization;

[TestClass]
public sealed class RequirePermissionAttributeTests
{
    [TestMethod]
    public void Constructor_ShouldBuildPermissionPolicy()
    {
        var attribute = new RequirePermissionAttribute("erp.skus.pricing.read");

        Assert.AreEqual("erp.skus.pricing.read", attribute.Permission);
        Assert.AreEqual("Permission:erp.skus.pricing.read", attribute.Policy);
    }

    [TestMethod]
    public void TryParsePolicy_ShouldReturnPermission_WhenPolicyMatchesPrefix()
    {
        var parsed = RequirePermissionAttribute.TryParsePolicy(
            "Permission:erp.skus.pricing.read",
            out var permission);

        Assert.IsTrue(parsed);
        Assert.AreEqual("erp.skus.pricing.read", permission);
    }

    [TestMethod]
    public void TryParsePolicy_ShouldReturnFalse_WhenPolicyDoesNotMatchPrefix()
    {
        var parsed = RequirePermissionAttribute.TryParsePolicy(
            "Other:erp.skus.pricing.read",
            out var permission);

        Assert.IsFalse(parsed);
        Assert.AreEqual(string.Empty, permission);
    }
}

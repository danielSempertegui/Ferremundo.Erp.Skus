using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Ferremundo.Erp.Skus.Api.Authorization;

public sealed class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        : base(options)
    {
    }

    public override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (!RequirePermissionAttribute.TryParsePolicy(policyName, out var permission))
        {
            return base.GetPolicyAsync(policyName);
        }

        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddRequirements(new PermissionRequirement(permission))
            .Build();

        return Task.FromResult<AuthorizationPolicy?>(policy);
    }
}

using Microsoft.AspNetCore.Authorization;

namespace Ferremundo.Erp.Skus.Api.Authorization;

public sealed class RequirePermissionAttribute : AuthorizeAttribute
{
    private const string PolicyPrefix = "Permission:";

    public RequirePermissionAttribute(string permission)
    {
        Permission = permission;
        Policy = $"{PolicyPrefix}{permission}";
    }

    public string Permission { get; }

    public static bool TryParsePolicy(string? policy, out string permission)
    {
        permission = string.Empty;

        if (string.IsNullOrWhiteSpace(policy) || !policy.StartsWith(PolicyPrefix, StringComparison.Ordinal))
        {
            return false;
        }

        permission = policy[PolicyPrefix.Length..];
        return !string.IsNullOrWhiteSpace(permission);
    }
}

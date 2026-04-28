using Ferremundo.Erp.Skus.Application.Abstractions.Security;

namespace Ferremundo.Erp.Skus.Api.Services;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetCurrentUserName()
        => _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "system";
}

using Ferremundo.Erp.Skus.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Ferremundo.Erp.Skus.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ISkuPricingAppService, SkuPricingAppService>();
        return services;
    }
}

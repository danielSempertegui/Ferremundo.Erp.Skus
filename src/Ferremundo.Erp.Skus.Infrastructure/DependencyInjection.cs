using Ferremundo.Erp.Skus.Application.Abstractions.Gateways;
using Ferremundo.Erp.Skus.Infrastructure.Ax2012.Credentials;
using Ferremundo.Erp.Skus.Infrastructure.Ax2012.Factories;
using Ferremundo.Erp.Skus.Infrastructure.Ax2012.Mappings;
using Ferremundo.Erp.Skus.Infrastructure.Ax2012.Options;
using Ferremundo.Erp.Skus.Infrastructure.Gateways;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ferremundo.Erp.Skus.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<Ax2012Options>()
            .Bind(configuration.GetSection(Ax2012Options.SectionName))
            .Validate(options => !string.IsNullOrWhiteSpace(options.PricingService.EndpointAddress), "Ax2012:PricingService:EndpointAddress is required.")
            .Validate(options => options.PricingService.SendTimeoutMinutes > 0, "Ax2012:PricingService:SendTimeoutMinutes must be greater than zero.")
            .ValidateOnStart();

        services.AddSingleton<IAxClientCredentialConfigurator, AxClientCredentialConfigurator>();
        services.AddSingleton<AxPricingServiceClientFactory>();
        services.AddScoped<AxSkuPricingRequestMapper>();
        services.AddScoped<ISkuPricingGateway, AxSkuPricingGateway>();

        return services;
    }
}

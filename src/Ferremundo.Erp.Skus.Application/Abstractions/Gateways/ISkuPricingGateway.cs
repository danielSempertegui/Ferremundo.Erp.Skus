using Ferremundo.Erp.Skus.Application.Commands.Skus.Pricing.GetSkuPricing;
using Ferremundo.Erp.Skus.Contracts.Skus.Responses;

namespace Ferremundo.Erp.Skus.Application.Abstractions.Gateways;

public interface ISkuPricingGateway
{
    Task<SkuPricingListResponse> GetAsync(
        GetSkuPricingCommand command,
        CancellationToken cancellationToken = default);
}

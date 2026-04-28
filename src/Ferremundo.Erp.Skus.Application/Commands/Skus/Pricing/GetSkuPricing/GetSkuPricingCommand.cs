using Ferremundo.Erp.Skus.Contracts.Skus.Enums;

namespace Ferremundo.Erp.Skus.Application.Commands.Skus.Pricing.GetSkuPricing;

public sealed class GetSkuPricingCommand
{
    public string? Sku { get; init; }

    public PricingMode Mode { get; init; }
}

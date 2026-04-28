using Ferremundo.Erp.Skus.Contracts.Skus.Requests;

namespace Ferremundo.Erp.Skus.Application.Commands.Skus.Pricing.GetSkuPricing;

public static class GetSkuPricingCommandMapper
{
    public static GetSkuPricingCommand Map(GetSkuPricingRequest request)
    {
        return new GetSkuPricingCommand
        {
            Mode = request.Mode!.Value
        };
    }

    public static GetSkuPricingCommand Map(GetSkuPricingBySkuRequest request)
    {
        return new GetSkuPricingCommand
        {
            Sku = request.Sku,
            Mode = request.Mode!.Value
        };
    }
}

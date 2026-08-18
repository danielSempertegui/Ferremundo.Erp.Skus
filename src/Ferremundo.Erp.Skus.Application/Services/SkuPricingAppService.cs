using Ferremundo.Erp.Skus.Application.Abstractions.Gateways;
using Ferremundo.Erp.Skus.Application.Abstractions.Services;
using Ferremundo.Erp.Skus.Application.Commands.Skus.Pricing.GetSkuPricing;
using Ferremundo.Erp.Skus.Contracts.Common;
using Ferremundo.Erp.Skus.Contracts.Skus;
using Ferremundo.Erp.Skus.Contracts.Skus.Requests;
using Ferremundo.Erp.Skus.Contracts.Skus.Responses;

namespace Ferremundo.Erp.Skus.Application.Services;

public sealed class SkuPricingAppService : ISkuPricingAppService
{
    private readonly ISkuPricingGateway _skuPricingGateway;

    public SkuPricingAppService(ISkuPricingGateway skuPricingGateway)
    {
        _skuPricingGateway = skuPricingGateway;
    }

    public async Task<ResponseBase<SkuPricingListResponse>> GetAsync(
        GetSkuPricingRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = GetSkuPricingCommandMapper.Map(request);
        var response = await _skuPricingGateway.GetAsync(command, cancellationToken);

        return ResponseFactory.Success(
            response,
            "SKU pricing retrieved successfully.",
            SkuResponseCodes.PricingRetrieved);
    }

    public async Task<ResponseBase<SkuPricingListResponse>> GetBySkuAsync(
        GetSkuPricingBySkuRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = GetSkuPricingCommandMapper.Map(request);
        var response = await _skuPricingGateway.GetAsync(command, cancellationToken);

        return ResponseFactory.Success(
            response,
            "SKU pricing retrieved successfully.",
            SkuResponseCodes.PricingRetrieved);
    }
}

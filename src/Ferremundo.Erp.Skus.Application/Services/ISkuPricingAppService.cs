using Ferremundo.Erp.Skus.Contracts.Common;
using Ferremundo.Erp.Skus.Contracts.Skus.Requests;
using Ferremundo.Erp.Skus.Contracts.Skus.Responses;

namespace Ferremundo.Erp.Skus.Application.Services;

public interface ISkuPricingAppService
{
    Task<ResponseBase<SkuPricingListResponse>> GetAsync(
        GetSkuPricingRequest request,
        CancellationToken cancellationToken = default);

    Task<ResponseBase<SkuPricingListResponse>> GetBySkuAsync(
        GetSkuPricingBySkuRequest request,
        CancellationToken cancellationToken = default);
}

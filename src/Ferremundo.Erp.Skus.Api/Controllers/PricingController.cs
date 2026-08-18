using Asp.Versioning;
using Ferremundo.Erp.Skus.Api.Authorization;
using Ferremundo.Erp.Skus.Application;
using Ferremundo.Erp.Skus.Application.Abstractions.Services;
using Ferremundo.Erp.Skus.Contracts.Common;
using Ferremundo.Erp.Skus.Contracts.Skus.Requests;
using Ferremundo.Erp.Skus.Contracts.Skus.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ferremundo.Erp.Skus.Api.Controllers;

[ApiController]
[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/skus")]
public sealed class PricingController : ControllerBase
{
    private readonly ISkuPricingAppService _skuPricingAppService;

    public PricingController(ISkuPricingAppService skuPricingAppService)
    {
        _skuPricingAppService = skuPricingAppService;
    }

    [HttpGet("pricing")]
    [RequirePermission(SkuPermissionCodes.PricingRead)]
    [ProducesResponseType(typeof(ResponseBase<SkuPricingListResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseBase<object?>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseBase<object?>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseBase<object?>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResponseBase<object?>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ResponseBase<SkuPricingListResponse>>> GetAsync(
        [FromQuery] GetSkuPricingRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _skuPricingAppService.GetAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpGet("{sku}/pricing")]
    [RequirePermission(SkuPermissionCodes.PricingRead)]
    [ProducesResponseType(typeof(ResponseBase<SkuPricingListResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseBase<object?>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseBase<object?>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseBase<object?>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResponseBase<object?>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ResponseBase<SkuPricingListResponse>>> GetBySkuAsync(
        [FromRoute] string sku,
        [FromQuery] GetSkuPricingRequest query,
        CancellationToken cancellationToken)
    {
        var request = new GetSkuPricingBySkuRequest
        {
            Sku = sku,
            Mode = query.Mode
        };

        var response = await _skuPricingAppService.GetBySkuAsync(request, cancellationToken);
        return Ok(response);
    }
}

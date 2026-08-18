using Ferremundo.Erp.Skus.Application.Abstractions.Gateways;
using Ferremundo.Erp.Skus.Application.Commands.Skus.Pricing.GetSkuPricing;
using Ferremundo.Erp.Skus.Application.Services;
using Ferremundo.Erp.Skus.Contracts.Skus;
using Ferremundo.Erp.Skus.Contracts.Skus.Enums;
using Ferremundo.Erp.Skus.Contracts.Skus.Requests;
using Ferremundo.Erp.Skus.Contracts.Skus.Responses;

namespace Ferremundo.Erp.Skus.Application.Tests.Services;

[TestClass]
public sealed class SkuPricingAppServiceTests
{
    [TestMethod]
    public async Task GetAsync_ShouldMapRequestAndReturnSuccessResponse()
    {
        var gateway = new CapturingSkuPricingGateway(CreateResponse());
        var service = new SkuPricingAppService(gateway);
        var request = new GetSkuPricingRequest
        {
            Mode = PricingMode.File
        };

        var response = await service.GetAsync(request);

        Assert.IsTrue(response.Success);
        Assert.AreEqual(SkuResponseCodes.PricingRetrieved, response.Code);
        Assert.AreSame(gateway.Response, response.Data);
        Assert.AreEqual(PricingMode.File, gateway.LastCommand?.Mode);
        Assert.IsNull(gateway.LastCommand?.Sku);
    }

    [TestMethod]
    public async Task GetBySkuAsync_ShouldMapSkuAndModeBeforeCallingGateway()
    {
        var gateway = new CapturingSkuPricingGateway(CreateResponse());
        var service = new SkuPricingAppService(gateway);
        var request = new GetSkuPricingBySkuRequest
        {
            Sku = "0025645",
            Mode = PricingMode.Data
        };

        var response = await service.GetBySkuAsync(request);

        Assert.IsTrue(response.Success);
        Assert.AreEqual(SkuResponseCodes.PricingRetrieved, response.Code);
        Assert.AreEqual("0025645", gateway.LastCommand?.Sku);
        Assert.AreEqual(PricingMode.Data, gateway.LastCommand?.Mode);
    }

    private static SkuPricingListResponse CreateResponse()
        => new()
        {
            Count = 1,
            Items =
            [
                new SkuPricingItemResponse
                {
                    Sku = "0025645",
                    Mode = PricingMode.File,
                    Price = 10.50m,
                    Percent1 = 0m,
                    QuantityAmountFrom = 1m,
                    UnitId = "UND"
                }
            ]
        };

    private sealed class CapturingSkuPricingGateway : ISkuPricingGateway
    {
        public CapturingSkuPricingGateway(SkuPricingListResponse response)
        {
            Response = response;
        }

        public SkuPricingListResponse Response { get; }

        public GetSkuPricingCommand? LastCommand { get; private set; }

        public Task<SkuPricingListResponse> GetAsync(
            GetSkuPricingCommand command,
            CancellationToken cancellationToken = default)
        {
            LastCommand = command;
            return Task.FromResult(Response);
        }
    }
}

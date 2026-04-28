using Ferremundo.Erp.Skus.Application.Commands.Skus.Pricing.GetSkuPricing;
using Ferremundo.Erp.Skus.Contracts.Skus.Enums;
using Ferremundo.Erp.Skus.Contracts.Skus.Requests;

namespace Ferremundo.Erp.Skus.Application.Tests.Commands.Skus.Pricing.GetSkuPricing;

[TestClass]
public sealed class GetSkuPricingCommandMapperTests
{
    [TestMethod]
    public void Map_FromListRequest_ShouldPopulateCommand()
    {
        var request = new GetSkuPricingRequest
        {
            Mode = PricingMode.File
        };

        var command = GetSkuPricingCommandMapper.Map(request);

        Assert.AreEqual(PricingMode.File, command.Mode);
        Assert.IsNull(command.Sku);
    }

    [TestMethod]
    public void Map_FromBySkuRequest_ShouldPopulateCommand()
    {
        var request = new GetSkuPricingBySkuRequest
        {
            Sku = "0025645",
            Mode = PricingMode.Data
        };

        var command = GetSkuPricingCommandMapper.Map(request);

        Assert.AreEqual("0025645", command.Sku);
        Assert.AreEqual(PricingMode.Data, command.Mode);
    }
}

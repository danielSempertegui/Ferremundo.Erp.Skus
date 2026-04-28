using Ferremundo.Erp.Skus.Application.Commands.Skus.Pricing.GetSkuPricing;
using Ferremundo.Erp.Skus.Contracts.Skus.Enums;
using Ferremundo.Erp.Skus.Infrastructure.Configuration;
using Ferremundo.Erp.Skus.Infrastructure.Mappings.Ax2012;
using Microsoft.Extensions.Options;

namespace Ferremundo.Erp.Skus.Application.Tests.Infrastructure.Mappings.Ax2012;

[TestClass]
public sealed class AxSkuPricingRequestMapperTests
{
    [TestMethod]
    public void Map_ShouldBuildAxQuery()
    {
        var options = Options.Create(new Ax2012Options
        {
            PricingService = new Ax2012ServiceOptions
            {
                DefaultCompany = "fm",
                Language = "es",
                PartitionKey = "initial"
            }
        });

        var mapper = new AxSkuPricingRequestMapper(options);
        var command = new GetSkuPricingCommand
        {
            Sku = "0025645",
            Mode = PricingMode.File
        };

        var result = mapper.Map(command);

        Assert.AreEqual("fm", result.CallContext.Company);
        Assert.AreEqual("es", result.CallContext.Language);
        Assert.AreEqual("initial", result.CallContext.PartitionKey);
        Assert.AreEqual("0025645", result.Request.ItemId);
        Assert.AreEqual(ClientConsolaAX.Console.FERRPriceServiceGroup.FERRPriceServiceMode.Fichero, result.Request.Mode);
    }
}

using Ferremundo.Erp.Skus.Application.Commands.Skus.Pricing.GetSkuPricing;
using Ferremundo.Erp.Skus.Contracts.Skus.Enums;
using Ferremundo.Erp.Skus.Infrastructure.Ax2012.Options;
using Microsoft.Extensions.Options;

namespace Ferremundo.Erp.Skus.Infrastructure.Ax2012.Mappings;

public sealed class AxSkuPricingRequestMapper
{
    private readonly Ax2012Options _options;

    public AxSkuPricingRequestMapper(IOptions<Ax2012Options> options)
    {
        _options = options.Value;
    }

    public AxPricingServiceQuery Map(GetSkuPricingCommand command)
    {
        var pricingOptions = _options.PricingService;

        return new AxPricingServiceQuery
        {
            CallContext = new Service.CallContext
            {
                Company = pricingOptions.DefaultCompany,
                Language = pricingOptions.Language,
                PartitionKey = pricingOptions.PartitionKey
            },
            Request = new Service.FERRPriceServiceRequest
            {
                ItemId = command.Sku ?? string.Empty,
                Mode = MapMode(command.Mode)
            }
        };
    }

    private static Service.FERRPriceServiceMode MapMode(PricingMode mode)
        => mode == PricingMode.File
            ? Service.FERRPriceServiceMode.Fichero
            : Service.FERRPriceServiceMode.Data;
}

namespace Ferremundo.Erp.Skus.Infrastructure.Ax2012.Mappings;

public sealed class AxPricingServiceQuery
{
    public required Service.CallContext CallContext { get; init; }

    public required Service.FERRPriceServiceRequest Request { get; init; }
}

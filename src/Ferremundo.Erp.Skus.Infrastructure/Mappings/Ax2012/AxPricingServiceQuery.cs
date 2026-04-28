namespace Ferremundo.Erp.Skus.Infrastructure.Mappings.Ax2012;

public sealed class AxPricingServiceQuery
{
    public required Service.CallContext CallContext { get; init; }

    public required Service.FERRPriceServiceRequest Request { get; init; }
}

namespace Ferremundo.Erp.Skus.Infrastructure.Security;

public interface IAxClientCredentialConfigurator
{
    void Configure(Service.FERRPriceServiceClient client);
}

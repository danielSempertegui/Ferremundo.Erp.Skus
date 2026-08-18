namespace Ferremundo.Erp.Skus.Infrastructure.Ax2012.Credentials;

public interface IAxClientCredentialConfigurator
{
    void Configure(Service.FERRPriceServiceClient client);
}

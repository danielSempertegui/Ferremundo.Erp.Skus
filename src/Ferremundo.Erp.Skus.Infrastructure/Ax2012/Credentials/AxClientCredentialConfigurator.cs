using Ferremundo.Erp.Skus.Application.Exceptions;
using Ferremundo.Erp.Skus.Contracts.Common;
using Ferremundo.Erp.Skus.Infrastructure.Ax2012.Options;
using Microsoft.Extensions.Options;
using System.Net;

namespace Ferremundo.Erp.Skus.Infrastructure.Ax2012.Credentials;

public sealed class AxClientCredentialConfigurator : IAxClientCredentialConfigurator
{
    private readonly Ax2012Options _options;

    public AxClientCredentialConfigurator(IOptions<Ax2012Options> options)
    {
        _options = options.Value;
    }

    public void Configure(Service.FERRPriceServiceClient client)
    {
        var pricingOptions = _options.PricingService;

        switch (pricingOptions.CredentialMode)
        {
            case AxCredentialMode.None:
            case AxCredentialMode.DefaultWindows:
                return;

            case AxCredentialMode.ExplicitWindows:
                if (string.IsNullOrWhiteSpace(pricingOptions.UserName) ||
                    string.IsNullOrWhiteSpace(pricingOptions.Password))
                {
                    throw new AxServiceException(
                        ResponseCodes.BadGateway,
                        "AX explicit credentials are configured incorrectly.");
                }

                client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(
                    pricingOptions.UserName,
                    pricingOptions.Password,
                    pricingOptions.DefaultDomain);
                return;

            default:
                throw new AxServiceException(
                    ResponseCodes.BadGateway,
                    $"Unsupported AX credential mode: {pricingOptions.CredentialMode}.");
        }
    }
}

using Ferremundo.Erp.Skus.Application.Exceptions;
using Ferremundo.Erp.Skus.Contracts.Common;
using Ferremundo.Erp.Skus.Infrastructure.Configuration;
using Ferremundo.Erp.Skus.Infrastructure.Security;
using Microsoft.Extensions.Options;
using System.ServiceModel;
using System.Xml;

namespace Ferremundo.Erp.Skus.Infrastructure;

public sealed class AxPricingServiceClientFactory
{
    private readonly Ax2012Options _options;
    private readonly IAxClientCredentialConfigurator _credentialConfigurator;

    public AxPricingServiceClientFactory(
        IOptions<Ax2012Options> options,
        IAxClientCredentialConfigurator credentialConfigurator)
    {
        _options = options.Value;
        _credentialConfigurator = credentialConfigurator;
    }

    public Service.FERRPriceServiceClient Create()
    {
        var pricingOptions = _options.PricingService;

        if (string.IsNullOrWhiteSpace(pricingOptions.EndpointAddress))
        {
            throw new AxServiceException(
                ResponseCodes.BadGateway,
                "AX pricing service endpoint address is required.");
        }

        if (pricingOptions.SendTimeoutMinutes <= 0)
        {
            throw new AxServiceException(
                ResponseCodes.BadGateway,
                "AX pricing service send timeout must be greater than zero.");
        }

        var binding = new NetTcpBinding
        {
            SendTimeout = TimeSpan.FromMinutes(pricingOptions.SendTimeoutMinutes),
            MaxBufferSize = int.MaxValue,
            MaxReceivedMessageSize = int.MaxValue,
            ReaderQuotas = XmlDictionaryReaderQuotas.Max
        };

        var endpoint = string.IsNullOrWhiteSpace(pricingOptions.UpnIdentity)
            ? new EndpointAddress(new Uri(pricingOptions.EndpointAddress))
            : new EndpointAddress(
                new Uri(pricingOptions.EndpointAddress),
                new UpnEndpointIdentity(pricingOptions.UpnIdentity));

        var client = new Service.FERRPriceServiceClient(binding, endpoint);
        _credentialConfigurator.Configure(client);

        return client;
    }
}

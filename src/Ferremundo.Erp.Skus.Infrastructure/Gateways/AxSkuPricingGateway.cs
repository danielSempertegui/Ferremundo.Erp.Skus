using Ferremundo.Erp.Skus.Application.Abstractions.Gateways;
using Ferremundo.Erp.Skus.Application.Commands.Skus.Pricing.GetSkuPricing;
using Ferremundo.Erp.Skus.Application.Exceptions;
using Ferremundo.Erp.Skus.Contracts.Common;
using Ferremundo.Erp.Skus.Contracts.Skus.Enums;
using Ferremundo.Erp.Skus.Contracts.Skus.Responses;
using Ferremundo.Erp.Skus.Infrastructure.Ax2012.Factories;
using Ferremundo.Erp.Skus.Infrastructure.Ax2012.Mappings;
using Microsoft.Extensions.Logging;
using System.ServiceModel;

namespace Ferremundo.Erp.Skus.Infrastructure.Gateways;

public sealed class AxSkuPricingGateway : ISkuPricingGateway
{
    private readonly AxPricingServiceClientFactory _clientFactory;
    private readonly AxSkuPricingRequestMapper _requestMapper;
    private readonly ILogger<AxSkuPricingGateway> _logger;

    public AxSkuPricingGateway(
        AxPricingServiceClientFactory clientFactory,
        AxSkuPricingRequestMapper requestMapper,
        ILogger<AxSkuPricingGateway> logger)
    {
        _clientFactory = clientFactory;
        _requestMapper = requestMapper;
        _logger = logger;
    }

    public async Task<SkuPricingListResponse> GetAsync(
        GetSkuPricingCommand command,
        CancellationToken cancellationToken = default)
    {
        var query = _requestMapper.Map(command);
        var client = _clientFactory.Create();

        try
        {
            _logger.LogInformation(
                "Calling AX pricing service. Mode: {Mode}. Sku: {Sku}",
                command.Mode,
                command.Sku ?? "ALL");

            await client.OpenAsync();
            var providerResponse = await client.getItemsAsync(query.CallContext, query.Request);

            var items = providerResponse.response ?? [];

            var result = new SkuPricingListResponse
            {
                Count = items.Length,
                Items = items
                    .Select(item => new SkuPricingItemResponse
                    {
                        Sku = item.ItemId,
                        Mode = MapMode(item.Mode),
                        Percent1 = item.Percent1,
                        Price = item.Price,
                        QuantityAmountFrom = item.QuantityAmountFrom,
                        UnitId = item.UnitId
                    })
                    .ToArray()
            };

            await client.CloseAsync();

            return result;
        }
        catch (FaultException<Service.AifFault> exception)
        {
            client.Abort();

            throw new AxServiceException(
                ResponseCodes.BadGateway,
                $"AX pricing service rejected the request: {exception.Message}",
                providerError: exception.Detail,
                innerException: exception);
        }
        catch (TimeoutException exception)
        {
            client.Abort();

            throw new AxServiceException(
                ResponseCodes.BadGateway,
                "AX pricing service timed out while processing the request.",
                providerError: new { exception.Message },
                innerException: exception);
        }
        catch (CommunicationException exception)
        {
            client.Abort();

            throw new AxServiceException(
                ResponseCodes.BadGateway,
                "AX pricing service communication failed.",
                providerError: new { exception.Message },
                innerException: exception);
        }
        catch
        {
            client.Abort();
            throw;
        }
    }

    private static PricingMode MapMode(Service.FERRPriceServiceMode mode)
        => mode == Service.FERRPriceServiceMode.Fichero ? PricingMode.File : PricingMode.Data;
}

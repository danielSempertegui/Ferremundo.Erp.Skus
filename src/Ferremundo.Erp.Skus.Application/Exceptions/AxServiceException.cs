using Microsoft.AspNetCore.Http;

namespace Ferremundo.Erp.Skus.Application.Exceptions;

public sealed class AxServiceException : Exception
{
    public AxServiceException(
        string code,
        string message,
        int statusCode = StatusCodes.Status502BadGateway,
        object? providerError = null,
        Exception? innerException = null)
        : base(message, innerException)
    {
        Code = code;
        StatusCode = statusCode;
        ProviderError = providerError;
    }

    public string Code { get; }

    public int StatusCode { get; }

    public object? ProviderError { get; }
}

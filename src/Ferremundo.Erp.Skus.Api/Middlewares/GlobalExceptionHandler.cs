using Ferremundo.Erp.Skus.Application.Exceptions;
using Ferremundo.Erp.Skus.Contracts.Common;
using Microsoft.AspNetCore.Diagnostics;

namespace Ferremundo.Erp.Skus.Api.Middlewares;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IHostEnvironment _environment;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger,
        IHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        LogException(httpContext, exception);

        var (statusCode, response) = MapException(exception);

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";

        if (_environment.IsDevelopment())
        {
            response = new ResponseBase<object?>
            {
                Success = response.Success,
                Code = response.Code,
                Message = response.Message,
                Data = new
                {
                    path = httpContext.Request.Path.Value,
                    method = httpContext.Request.Method,
                    exceptionType = exception.GetType().FullName,
                    exceptionMessage = exception.Message,
                    innerExceptionMessage = exception.InnerException?.Message,
                    providerError = (exception as AxServiceException)?.ProviderError
                }
            };
        }

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }

    private void LogException(HttpContext httpContext, Exception exception)
    {
        var path = httpContext.Request.Path.Value ?? string.Empty;
        var method = httpContext.Request.Method;
        var userName = httpContext.User?.Identity?.Name ?? "anonymous";

        switch (exception)
        {
            case ValidationException validationException:
                _logger.LogWarning(
                    "Validation error. Code: {Code}. Method: {Method}. Path: {Path}. User: {UserName}. Message: {Message}",
                    validationException.Code,
                    method,
                    path,
                    userName,
                    validationException.Message);
                break;

            case NotFoundException notFoundException:
                _logger.LogWarning(
                    "Resource not found. Code: {Code}. Method: {Method}. Path: {Path}. User: {UserName}. Message: {Message}",
                    notFoundException.Code,
                    method,
                    path,
                    userName,
                    notFoundException.Message);
                break;

            case BusinessException businessException:
                _logger.LogWarning(
                    "Business error. Code: {Code}. Method: {Method}. Path: {Path}. User: {UserName}. Message: {Message}",
                    businessException.Code,
                    method,
                    path,
                    userName,
                    businessException.Message);
                break;

            case AxServiceException axServiceException:
                _logger.LogWarning(
                    axServiceException,
                    "AX service error. Code: {Code}. StatusCode: {StatusCode}. Method: {Method}. Path: {Path}. User: {UserName}. Message: {Message}",
                    axServiceException.Code,
                    axServiceException.StatusCode,
                    method,
                    path,
                    userName,
                    axServiceException.Message);
                break;

            default:
                _logger.LogError(
                    exception,
                    "Unhandled exception. Method: {Method}. Path: {Path}. User: {UserName}",
                    method,
                    path,
                    userName);
                break;
        }
    }

    private static (int StatusCode, ResponseBase<object?> Response) MapException(Exception exception)
    {
        return exception switch
        {
            ValidationException validationException => (
                StatusCodes.Status400BadRequest,
                ResponseFactory.Fail<object?>(validationException.Code, validationException.Message)
            ),

            NotFoundException notFoundException => (
                StatusCodes.Status404NotFound,
                ResponseFactory.Fail<object?>(notFoundException.Code, notFoundException.Message)
            ),

            BusinessException businessException => (
                StatusCodes.Status409Conflict,
                ResponseFactory.Fail<object?>(businessException.Code, businessException.Message)
            ),

            AxServiceException axServiceException => (
                axServiceException.StatusCode,
                ResponseFactory.Fail<object?>(axServiceException.Code, axServiceException.Message)
            ),

            _ => (
                StatusCodes.Status500InternalServerError,
                ResponseFactory.Fail<object?>(ResponseCodes.InternalServerError, "An unexpected error occurred.")
            )
        };
    }
}

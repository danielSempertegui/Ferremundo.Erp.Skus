using Ferremundo.Erp.Skus.Contracts.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Ferremundo.Erp.Skus.Api.Extensions;

public static class AuthenticationResponseWriter
{
    public static Task HandleChallengeAsync(JwtBearerChallengeContext context)
    {
        context.HandleResponse();

        return WriteFailureAsync(
            context.Response,
            StatusCodes.Status401Unauthorized,
            ResponseCodes.Unauthorized,
            "Authentication is required to access this resource.",
            context.HttpContext.RequestAborted);
    }

    public static Task HandleForbiddenAsync(ForbiddenContext context)
    {
        return WriteFailureAsync(
            context.Response,
            StatusCodes.Status403Forbidden,
            ResponseCodes.Forbidden,
            "You do not have permission to access this resource.",
            context.HttpContext.RequestAborted);
    }

    private static Task WriteFailureAsync(
        HttpResponse response,
        int statusCode,
        string code,
        string message,
        CancellationToken cancellationToken)
    {
        if (response.HasStarted)
        {
            return Task.CompletedTask;
        }

        response.StatusCode = statusCode;
        response.ContentType = "application/json";

        return response.WriteAsJsonAsync(
            ResponseFactory.Fail<object?>(code, message),
            cancellationToken);
    }
}

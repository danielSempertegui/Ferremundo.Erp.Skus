using Ferremundo.Erp.Skus.Contracts.Common;
using Microsoft.AspNetCore.Mvc;

namespace Ferremundo.Erp.Skus.Api.Extensions;

public static class ApiBehaviorExtensions
{
    public static IServiceCollection AddCustomApiBehavior(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var validationErrors = context.ModelState
                    .Where(modelState => modelState.Value?.Errors.Count > 0)
                    .Select(modelState => new ValidationErrorResponse
                    {
                        Field = modelState.Key,
                        Errors = modelState.Value!.Errors
                            .Select(error => string.IsNullOrWhiteSpace(error.ErrorMessage)
                                ? "The field is invalid."
                                : error.ErrorMessage)
                            .ToArray()
                    })
                    .ToArray();

                var response = ResponseFactory.Fail<IReadOnlyCollection<ValidationErrorResponse>>(
                    ResponseCodes.ValidationError,
                    "One or more validation errors occurred.");

                response = new ResponseBase<IReadOnlyCollection<ValidationErrorResponse>>
                {
                    Success = response.Success,
                    Code = response.Code,
                    Message = response.Message,
                    Data = validationErrors
                };

                return new BadRequestObjectResult(response);
            };
        });

        return services;
    }
}

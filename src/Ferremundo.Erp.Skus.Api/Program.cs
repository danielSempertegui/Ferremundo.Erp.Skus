using Asp.Versioning;
using Ferremundo.Erp.Skus.Api.Authorization;
using Ferremundo.Erp.Skus.Api.Configuration;
using Ferremundo.Erp.Skus.Api.Extensions;
using Ferremundo.Erp.Skus.Api.Middlewares;
using Ferremundo.Erp.Skus.Api.Services;
using Ferremundo.Erp.Skus.Application;
using Ferremundo.Erp.Skus.Application.Abstractions.Security;
using Ferremundo.Erp.Skus.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using OpenIddict.Validation.AspNetCore;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Services.AddSerilog();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddCustomApiBehavior();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

var securityTokenValidationOptions = builder.Configuration
    .GetSection(SecurityTokenValidationOptions.SectionName)
    .Get<SecurityTokenValidationOptions>()
    ?? throw new InvalidOperationException($"{SecurityTokenValidationOptions.SectionName} configuration is required.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
});

builder.Services.AddOpenIddict()
    .AddValidation(options =>
    {
        options.SetIssuer(new Uri(securityTokenValidationOptions.Issuer));
        options.AddAudiences(securityTokenValidationOptions.Audience);
        options.SetClientId(securityTokenValidationOptions.ClientId);
        options.SetClientSecret(securityTokenValidationOptions.ClientSecret);
        options.UseIntrospection();
        options.UseSystemNetHttp();
        options.UseAspNetCore();
    });

builder.Services.AddAuthorization();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, ResponseAuthorizationMiddlewareResultHandler>();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("QA"))
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseWhen(
    context => context.Request.Path.StartsWithSegments("/api"),
    branch =>
    {
        branch.UseSerilogRequestLogging(options =>
        {
            options.GetLevel = (httpContext, _, _) =>
            {
                if (httpContext.Response.StatusCode >= 500)
                {
                    return LogEventLevel.Error;
                }

                if (httpContext.Response.StatusCode >= 400)
                {
                    return LogEventLevel.Warning;
                }

                return LogEventLevel.Information;
            };

            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set("UserName", httpContext.User?.Identity?.Name ?? "anonymous");
                diagnosticContext.Set("RequestPath", httpContext.Request.Path.Value ?? string.Empty);
                diagnosticContext.Set("RequestMethod", httpContext.Request.Method);
            };
        });
    });

app.UseExceptionHandler();

app.MapControllers();

app.Run();

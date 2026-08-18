namespace Ferremundo.Erp.Skus.Api.Configuration;

public sealed class SecurityTokenValidationOptions
{
    public const string SectionName = "SecurityTokenValidation";

    public string Issuer { get; init; } = string.Empty;

    public string Audience { get; init; } = string.Empty;

    public string ClientId { get; init; } = string.Empty;

    public string ClientSecret { get; init; } = string.Empty;
}

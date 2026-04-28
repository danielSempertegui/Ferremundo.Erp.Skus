namespace Ferremundo.Erp.Skus.Infrastructure.Configuration;

public sealed class Ax2012ServiceOptions
{
    public string EndpointAddress { get; set; } = string.Empty;

    public string UpnIdentity { get; set; } = string.Empty;

    public string DefaultCompany { get; set; } = "dat";

    public string DefaultDomain { get; set; } = string.Empty;

    public string? Language { get; set; }

    public string? PartitionKey { get; set; }

    public int SendTimeoutMinutes { get; set; } = 5;

    public AxCredentialMode CredentialMode { get; set; } = AxCredentialMode.None;

    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}

namespace Ferremundo.Erp.Skus.Infrastructure.Configuration;

public sealed class Ax2012Options
{
    public const string SectionName = "Ax2012";

    public Ax2012ServiceOptions PricingService { get; set; } = new();
}

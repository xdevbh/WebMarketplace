using Volo.Abp.Identity;

namespace WebMarketplace;

public static class WebMarketplaceConsts
{
    public const string DbTablePrefix = "App";
    public const string? DbSchema = null;
    public const string AdminEmailDefaultValue = IdentityDataSeedContributor.AdminEmailDefaultValue;
    public const string AdminPasswordDefaultValue = IdentityDataSeedContributor.AdminPasswordDefaultValue;

    public const int CurrencyCodeLength = 3;
    public const double RatingMinValue = 0;
    public const double RatingMaxValue = 5;
}

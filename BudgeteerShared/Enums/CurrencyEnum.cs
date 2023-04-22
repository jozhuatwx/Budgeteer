namespace BudgeteerShared.Enums;

public enum CurrencyEnum
{
    MYR,
    GBP,
    EUR,
    SGD,
    USD
}

public static class CurrencyEnumExtensions
{
    public static string ToSymbol(this CurrencyEnum currency)
    {
        return currency switch
        {
            CurrencyEnum.MYR => "RM",
            CurrencyEnum.GBP => "£",
            CurrencyEnum.EUR => "€",
            _ => "$"
        };
    }
}


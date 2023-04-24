namespace BudgeteerShared.Models;

public class Money
{
    public CurrencyEnum Currency { get; set; }
    public decimal Value { get; set; }

    public Money(CurrencyEnum currency, decimal value)
    {
        Currency = currency;
        Value = value;
    }

    public override string ToString()
    {
        return $"{Currency.ToSymbol()} {Value:0.00}";
    }
}


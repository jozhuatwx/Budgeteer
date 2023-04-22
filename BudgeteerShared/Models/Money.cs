namespace BudgeteerShared.Models;

public class Money
{
	public CurrencyEnum Currency { get; set; }
	public decimal Value { get; set; }

    public override string ToString()
    {
        return $"{Currency.ToSymbol()} {Value:0.00}";
    }
}


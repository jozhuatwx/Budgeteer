namespace BudgeteerShared.Models;

public class Account
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required CurrencyEnum Currency { get; set; }

    [SetsRequiredMembers]
    public Account(string name, CurrencyEnum currency)
    {
        Name = name;
        Currency = currency;
    }
}


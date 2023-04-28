namespace BudgeteerShared.FormModels;

public class AddTransactionViewFormModel
{
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public string Name { get; set; } = string.Empty;
    public CurrencyEnum Currency { get; set; }
    public decimal Amount { get; set; } = 0;
    public int AccountId { get; set; } = 1;
    public int CategoryId { get; set; } = 1;
}


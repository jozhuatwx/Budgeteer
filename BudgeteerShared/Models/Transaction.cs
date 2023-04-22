namespace BudgeteerShared.Models;

public class Transaction
{
	public DateTime Timestamp { get; set; }
	public string Name { get; set; }
	public Money Amount { get; set; }

	public Money OriginalAmount { get; set; }

	public string Category { get; set; }
	public string Notes { get; set; }
}


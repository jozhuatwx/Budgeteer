namespace BudgeteerShared.Models;

public class Account
{
	public string Name { get; set; }
	public Money Balance { get; set; }

	public List<Transaction> Transactions { get; set; }
}


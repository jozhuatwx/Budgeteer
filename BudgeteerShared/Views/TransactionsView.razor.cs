namespace BudgeteerShared.Views;

public partial class TransactionsView
{
	[Parameter]
	public List<Transaction> Transactions { get; set; }

	public TransactionsView()
	{
	}
}


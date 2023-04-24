namespace BudgeteerShared.Views;

public partial class TransactionsView
{
    [Parameter]
    public required IEnumerable<Transaction> Transactions { get; set; }
}


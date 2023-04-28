namespace BudgeteerShared.Views;

public partial class TransactionsView
{
    [Parameter]
    public required List<Transaction> Transactions { get; set; }
}


namespace BudgeteerShared.Views;

public partial class TransactionsView
{
    [Parameter]
    public required ObservableCollection<Transaction> Transactions { get; set; }
}


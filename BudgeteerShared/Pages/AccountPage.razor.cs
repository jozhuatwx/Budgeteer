namespace BudgeteerShared.Pages;

public partial class AccountPage
{
    [Inject]
    private IAccountsService AccountService { get; set; } = null!;

    [Inject]
    private ITransactionsService TransactionsService { get; set; } = null!;

    private Account? Account { get; set; }
    private Money? Balance { get; set; }
    private ObservableCollection<Transaction>? Transactions { get; set; }

    protected override void OnInitialized()
    {
        Account = AccountService.GetAccounts().FirstOrDefault();
        if (Account != null)
        {
            Transactions = TransactionsService.GetTransactions();
            Balance = new(Account.Currency, Transactions.Sum(t => t.Category!.IsDebit ? t.Amount.Value : -t.Amount.Value));
            Transactions.CollectionChanged += TransactionsCollectionChanged;
        }
    }

    private void TransactionsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        Balance!.Value = Transactions!.Sum(t => t.Category!.IsDebit ? t.Amount.Value : -t.Amount.Value);
        StateHasChanged();
    }
}


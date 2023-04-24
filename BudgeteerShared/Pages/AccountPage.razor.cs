namespace BudgeteerShared.Pages;

public partial class AccountPage
{
    public Account? Account { get; set; }
    public Money? Balance { get; set; }

    public IEnumerable<Transaction>? Transactions { get; set; }

    [Inject]
    private IAccountsService AccountService { get; set; } = null!;

    [Inject]
    public ITransactionsService TransactionsService { get; set; } = null!;

    protected override void OnInitialized()
    {
        Account = AccountService.GetAccounts().First();
        Transactions = TransactionsService.GetTransactionsByAccountId(Account.Id);
        Balance = new(Account.Currency, Transactions.Sum(t => t.Category!.IsDebit ? t.Amount.Value : -t.Amount.Value));
    }
}


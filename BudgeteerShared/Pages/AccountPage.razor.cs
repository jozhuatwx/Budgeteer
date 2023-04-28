namespace BudgeteerShared.Pages;

public partial class AccountPage
{
    public Account? Account { get; set; }
    public Money? Balance { get; set; }

    public List<Transaction>? Transactions { get; set; }

    [Inject]
    private IAccountsService AccountService { get; set; } = null!;

    [Inject]
    private ITransactionsService TransactionsService { get; set; } = null!;

    protected override void OnInitialized()
    {
        Account = AccountService.GetAccounts().FirstOrDefault();
        if (Account != null)
        {
            Transactions = TransactionsService.GetTransactionsByAccountId(Account.Id);
            Balance = new(Account.Currency, Transactions.Sum(t => t.Category!.IsDebit ? t.Amount.Value : -t.Amount.Value));
        }
    }
}


namespace BudgeteerApp.Services;

public class AccountsService : IAccountsService
{
    private IEnumerable<Account> Accounts { get; set; }

    public AccountsService(ITransactionsService transactionsService)
    {
        Initialise(transactionsService);
    }

    private void Initialise(ITransactionsService transactionsService)
    {
        Accounts = new List<Account>()
        {
            new("HSBC", CurrencyEnum.MYR)
            {
                Id = 1,
                Transactions = transactionsService.GetTransactionsByAccountId(1)
            }
        };
    }

    public Account GetAccount(int Id)
    {
        return Accounts.FirstOrDefault(a => a.Id == Id);
    }

    public IEnumerable<Account> GetAccounts()
    {
        return Accounts;
    }
}


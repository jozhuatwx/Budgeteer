namespace BudgeteerShared.Services;

public class TransactionsService : ITransactionsService
{
    private List<Transaction> Transactions { get; set; } = new();

    public TransactionsService(IAccountsService accountsService, ICategoriesService categoriesService)
    {
        Initialise(accountsService, categoriesService);
    }

    private void Initialise(IAccountsService accountsService, ICategoriesService categoriesService)
    {
        Transactions = new()
        {
            new("Deposit", new(CurrencyEnum.MYR, 100), accountsService.GetAccount(1)!, categoriesService.GetCategory(1)!)
            {
                Id = 1
            },
            new("McDonald's Lunch", new(CurrencyEnum.MYR, 10), accountsService.GetAccount(1)!, categoriesService.GetCategory(2)!)
            {
                Id = 2,
                OriginalAmount = new(CurrencyEnum.GBP, 1.9m)
            },
            new("McDonald's Breakfast", new(CurrencyEnum.MYR, 10), accountsService.GetAccount(1)!, categoriesService.GetCategory(2)!)
            {
                Id = 3,
                OriginalAmount = new(CurrencyEnum.GBP, 1.9m)
            }
        };
    }

    public Transaction? GetTransaction(int Id)
    {
        return Transactions.FirstOrDefault(t => t.Id == Id);
    }

    public List<Transaction> GetTransactions()
    {
        return Transactions;
    }

    public List<Transaction> GetTransactionsByAccountId(int accountId)
    {
        return Transactions.Where(t => t.AccountId == accountId).ToList();
    }
}


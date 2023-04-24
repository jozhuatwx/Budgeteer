namespace BudgeteerApp.Services;

public class TransactionsService : ITransactionsService
{
    private IEnumerable<Transaction> Transactions { get; set; }

    public TransactionsService(ICategoriesService categoriesService)
    {
        Initialise(categoriesService);
    }

    private void Initialise(ICategoriesService categoriesService)
    {
        Transactions = new List<Transaction>()
        {
            new("Deposit", new(CurrencyEnum.MYR, 100), 1, 1)
            {
                Id = 1,
                Category = categoriesService.GetCategory(1)
            },
            new("McDonald's Lunch", new(CurrencyEnum.MYR, 10), 1, 2)
            {
                Id = 2,
                OriginalAmount = new(CurrencyEnum.GBP, 1.9m),
                Category = categoriesService.GetCategory(2)
            },
            new("McDonald's Breakfast", new(CurrencyEnum.MYR, 10), 1, 2)
            {
                Id = 3,
                OriginalAmount = new(CurrencyEnum.GBP, 1.9m),
                Category = categoriesService.GetCategory(2)
            }
        };
    }

    public Transaction GetTransaction(int Id)
    {
        return Transactions.FirstOrDefault(t => t.Id == Id);
    }

    public IEnumerable<Transaction> GetTransactions()
    {
        return Transactions;
    }

    public IEnumerable<Transaction> GetTransactionsByAccountId(int accountId)
    {
        return Transactions.Where(t => t.AccountId == accountId);
    }
}


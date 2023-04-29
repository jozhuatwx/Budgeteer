namespace BudgeteerShared.Services;

public class TransactionsService : ITransactionsService
{
    private ObservableCollection<Transaction> Transactions { get; set; } = new();

    private IAccountsService AccountsService { get; set; }
    private ICategoriesService CategoriesService { get; set; }

    public TransactionsService(IAccountsService accountsService, ICategoriesService categoriesService)
    {
        CategoriesService = categoriesService;
        AccountsService = accountsService;
        Initialise();
    }

    private void Initialise()
    {
        Transactions = new()
        {
            new(DateTime.Now, "Deposit", new(CurrencyEnum.MYR, 100), AccountsService.GetAccount(1)!, CategoriesService.GetCategory(1)!)
            {
                Id = 1
            },
            new(DateTime.Now, "McDonald's Lunch", new(CurrencyEnum.MYR, 10), AccountsService.GetAccount(1)!, CategoriesService.GetCategory(2)!)
            {
                Id = 2,
                OriginalAmount = new(CurrencyEnum.GBP, 1.9m)
            },
            new(DateTime.Now, "McDonald's Breakfast", new(CurrencyEnum.MYR, 10), AccountsService.GetAccount(1)!, CategoriesService.GetCategory(2)!)
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

    public ObservableCollection<Transaction> GetTransactions()
    {
        return Transactions;
    }

    public Transaction AddTransaction(DateTime timestamp, string name, Money amount, int accountId, int categoryId)
    {
        var transaction = new Transaction(timestamp, name, amount, AccountsService.GetAccount(accountId)!, CategoriesService.GetCategory(categoryId)!)
        {
            Id = Transactions.Max(t => t.Id) + 1
        };
        Transactions.Add(transaction);
        return transaction;
    }
}


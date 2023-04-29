namespace BudgeteerShared.Services;

public interface ITransactionsService
{
    public Transaction? GetTransaction(int Id);
    public ObservableCollection<Transaction> GetTransactions();
    public Transaction AddTransaction(DateTime timestamp, string name, Money amount, int accountId, int categoryId);
}


namespace BudgeteerShared.Services;

public interface ITransactionsService
{
    public Transaction GetTransaction(int Id);
    public IEnumerable<Transaction> GetTransactions();
    public IEnumerable<Transaction> GetTransactionsByAccountId(int accountId);
}


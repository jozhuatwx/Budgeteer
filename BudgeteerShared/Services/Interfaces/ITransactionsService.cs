namespace BudgeteerShared.Services;

public interface ITransactionsService
{
    public Transaction? GetTransaction(int Id);
    public List<Transaction> GetTransactions();
    public List<Transaction> GetTransactionsByAccountId(int accountId);
}


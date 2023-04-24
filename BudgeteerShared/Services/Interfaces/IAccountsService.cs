namespace BudgeteerShared.Services;

public interface IAccountsService
{
    public Account? GetAccount(int Id);
    public List<Account> GetAccounts();
}


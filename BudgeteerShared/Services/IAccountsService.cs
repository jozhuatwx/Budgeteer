namespace BudgeteerShared.Services;

public interface IAccountsService
{
    Task<List<Account>> GetAccountsAsync();
}


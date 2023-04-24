namespace BudgeteerShared.Services;

public class AccountsService : IAccountsService
{
    private List<Account> Accounts { get; set; } = new();

    public AccountsService()
    {
        Initialise();
    }

    private void Initialise()
    {
        Accounts = new List<Account>()
        {
            new("HSBC", CurrencyEnum.MYR)
            {
                Id = 1
            }
        };
    }

    public Account? GetAccount(int Id)
    {
        return Accounts.FirstOrDefault(a => a.Id == Id);
    }

    public List<Account> GetAccounts()
    {
        return Accounts;
    }
}


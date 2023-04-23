namespace BudgeteerApp.Services;

public class AccountsService : IAccountsService
{
    public Task<List<Account>> GetAccountsAsync()
    {
        return Task.FromResult<List<Account>>(new()
        {
            new()
            {
                Name = "HSBC",
                Balance = new()
                {
                    Currency = CurrencyEnum.MYR,
                    Value = 1000
                },
                Transactions = new()
                {
                    new()
                    {
                        Timestamp = DateTime.Now,
                        Name = "McDonald's Lunch",
                        Amount = new()
                        {
                            Currency = CurrencyEnum.MYR,
                            Value = 10
                        },
                        OriginalAmount = new()
                        {
                            Currency = CurrencyEnum.GBP,
                            Value = 1.9m
                        },
                        Category = new()
                        {
                            Icon = "food",
                            Name = "Food"
                        }
                    },
                    new()
                    {
                        Timestamp = DateTime.Now,
                        Name = "McDonald's Breakfast",
                        Amount = new()
                        {
                            Currency = CurrencyEnum.MYR,
                            Value = 10
                        },
                        OriginalAmount = new()
                        {
                            Currency = CurrencyEnum.GBP,
                            Value = 1.9m
                        },
                        Category = new()
                        {
                            Icon = "food",
                            Name = "Food"
                        }
                    }
                }
            }
        });
    }
}


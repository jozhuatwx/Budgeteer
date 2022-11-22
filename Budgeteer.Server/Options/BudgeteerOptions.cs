namespace Budgeteer.Server.Options;

public class BudgeteerOptions
{
    public JwtOptions Jwt { get; set; } = new();
    public DatabaseOptions Database { get; set; } = new();
}

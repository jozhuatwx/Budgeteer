namespace BudgeteerShared.Models;

public class Category
{
    public int Id { get; set; }
    public required string Icon { get; set; }
    public required string Name { get; set; }
    public bool IsDebit { get; set; }

    [SetsRequiredMembers]
    public Category(string icon, string name, bool isDebit = false)
    {
        Icon = icon;
        Name = name;
        IsDebit = isDebit;
    }
}


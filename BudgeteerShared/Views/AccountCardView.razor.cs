namespace BudgeteerShared.Views;

public partial class AccountCardView
{
    [Parameter]
    public required string Name { get; set; }

    [Parameter]
    public required Money Balance { get; set; }
}


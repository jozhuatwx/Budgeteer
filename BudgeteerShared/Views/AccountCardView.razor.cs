namespace BudgeteerShared.Views;

public partial class AccountCardView
{
	[Parameter]
	public string Name { get; set; }

	[Parameter]
	public Money Balance { get; set; }

	public AccountCardView()
	{
	}
}


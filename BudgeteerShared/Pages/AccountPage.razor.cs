namespace BudgeteerShared.Pages;

public partial class AccountPage
{
	public Account? Account { get; set; }

    [Inject]
    private IAccountsService AccountService { get; set; }

	public AccountPage()
	{
	}

    protected override async Task OnInitializedAsync()
    {
        Account = (await AccountService.GetAccountsAsync())[0];
    }
}


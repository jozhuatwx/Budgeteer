namespace BudgeteerShared.Views;

public partial class AddTransactionView
{
    [Inject]
    private ITransactionsService TransactionsService { get; set; } = null!;

    [Inject]
    private ICategoriesService CategoriesService { get; set; } = null!;

    [Inject]
    private IAccountsService AccountsService { get; set; } = null!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = null!;

    private AddTransactionViewFormModel AddFormModel { get; set; } = new();

    private async void AddTransaction()
    {
        TransactionsService.AddTransaction(AddFormModel.Timestamp, AddFormModel.Name, new(AddFormModel.Currency, AddFormModel.Amount), AddFormModel.AccountId, AddFormModel.CategoryId);
        await JSRuntime.InvokeVoidAsync("toggleAddMenu");
    }
}


using System.ComponentModel.DataAnnotations;

namespace BudgeteerShared.FormModels;

public class AddTransactionViewFormModel
{
    [Required]
    public DateTime Timestamp { get; set; } = DateTime.Now;

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public CurrencyEnum Currency { get; set; }

    [Required]
    [Range(0, double.PositiveInfinity)]
    public decimal Amount { get; set; } = 0;

    [Required]
    public int AccountId { get; set; } = 1;

    [Required]
    public int CategoryId { get; set; } = 1;
}


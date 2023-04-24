namespace BudgeteerApp.Services;

public class CategoriesService : ICategoriesService
{
    private IEnumerable<Category> Categories { get; set; }

	public CategoriesService()
	{
        Initialise();
	}

    private void Initialise()
    {
        Categories = new List<Category>()
        {
            new("bank-transfer-in", "Bank-in", true)
            {
                Id = 1
            },
            new("food", "Food")
            {
                Id = 2
            }
        };
    }

    public Category GetCategory(int id)
    {
        return Categories.FirstOrDefault(c => c.Id == id);
    }

    public IEnumerable<Category> GetCategories()
    {
        return Categories;
    }
}


namespace BudgeteerShared.Services;

public interface ICategoriesService
{
    public Category GetCategory(int id);
    public IEnumerable<Category> GetCategories();
}


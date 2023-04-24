namespace BudgeteerShared.Services;

public interface ICategoriesService
{
    public Category? GetCategory(int id);
    public List<Category> GetCategories();
}


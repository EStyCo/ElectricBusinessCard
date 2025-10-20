using ElectricBusinessCard.Repository;
using ElectricBusinessCard.Services.EntityFramework.Models;

namespace ElectricBusinessCard.Services
{
    public class CategoryService(
        CategoryRepository _categoryRepos)
    {
        public async Task CreateNewCategory(string name, string description)
        {
            await _categoryRepos.AddCategoryAsync(name, description);
        }

        public async Task<List<CategoryWork>> GetAllCategoriesAsync()
        {
           return await _categoryRepos.GetAllCategoriesAsync();
        }

        public async Task DeleteCategory(int categoryIndex)
        {
            await _categoryRepos.DeleteCategoryAsync(categoryIndex);
        }
    }
}

using ElectricBusinessCard.Services.EntityFramework;
using ElectricBusinessCard.Services.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectricBusinessCard.Repository
{
    public class CategoryRepository(
        AppDbContext _dbContext)
    {
        public async Task<CategoryWork> GetCategoryByIndexAsync(int id)
        {
            return new();
        }

        public async Task<List<CategoryWork>> GetAllCategoriesAsync()
        {
            return _dbContext.CategoriesWorks
                .Include(x => x.Works)
                .ToList();
        }

        public async Task AddCategoryAsync(string name, string description)
        {
            var maxCount = _dbContext.CategoriesWorks.ToList().Count();

            await _dbContext.CategoriesWorks.AddAsync(new()
            {
                Name = name,
                Description = description,
                CategoryIndex = maxCount,
                Works = []
            });

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(CategoryWork category)
        {

        }

        public async Task DeleteCategoryAsync(int categoryIndex)
        {
            var category = await _dbContext.CategoriesWorks
                .FirstOrDefaultAsync(x => x.CategoryIndex == categoryIndex);
            if (category is null) return;

            _dbContext.CategoriesWorks.Remove(category);
            await _dbContext.SaveChangesAsync();
        }
    }
}

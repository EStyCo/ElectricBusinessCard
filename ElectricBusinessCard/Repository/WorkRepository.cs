using ElectricBusinessCard.Services.EntityFramework.Models;
using ElectricBusinessCard.Services.EntityFramework;
using ElectricBusinessCard.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace ElectricBusinessCard.Repository
{
    public class WorkRepository(
        AppDbContext _dbContext)
    {
        public async Task AddWorkAsync(ElectroWork dto, int categoryIndex)
        {
            var category = await _dbContext.CategoriesWorks
                .Include(c => c.Works)
                .FirstOrDefaultAsync(c => c.CategoryIndex == categoryIndex);

            if (category is null) return;
            var worksInCategory = category.Works.OrderBy(w => w.WorkIndex).ToList();

            for (int i = 0; i < worksInCategory.Count; i++)
            {
                worksInCategory[i].WorkIndex = i;
            }

            var work = new ElectroWork()
            {
                Name = dto.Name,
                Unit = dto.Unit,
                PriceInRubles = dto.PriceInRubles,
                Category = category,
                CategoryId = category.Id,
                Description = dto.Description ?? "",
                WorkIndex = worksInCategory.Count
            };

            await _dbContext.ElectroWorks.AddAsync(work);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteWorkAsync(int workIndex, int categoryIndex)
        {
            var category = await _dbContext.CategoriesWorks
                .Include(c => c.Works)
                .FirstOrDefaultAsync(c => c.CategoryIndex == categoryIndex);

            if (category == null) return;

            var workToDelete = category.Works.FirstOrDefault(w => w.WorkIndex == workIndex);
            if (workToDelete == null) return;

            _dbContext.ElectroWorks.Remove(workToDelete);

            var remainingWorks = category.Works
                .Where(w => w != workToDelete)
                .OrderBy(w => w.WorkIndex)
                .ToList();
            for (int i = 0; i < remainingWorks.Count; i++)
                remainingWorks[i].WorkIndex = i;

            await _dbContext.SaveChangesAsync();
        }

        public async Task MoveUpWork(int workIndex, int categoryIndex)
        {
            var category = await _dbContext.CategoriesWorks
                .Include(c => c.Works)
                .FirstOrDefaultAsync(x => x.CategoryIndex == categoryIndex);

            if (category is null)
                throw new ArgumentException("Категория не найдена");
            var currentWork = category.Works.FirstOrDefault(x => x.WorkIndex == workIndex);
            if (currentWork == null)
                throw new ArgumentException("Работа не найдена");

            int minIndex = category.Works.Min(w => w.WorkIndex);
            if (currentWork.WorkIndex == minIndex)
                return;

            var previousWork = category.Works.FirstOrDefault(x => x.WorkIndex == currentWork.WorkIndex - 1);
            if (previousWork == null)
                return;

            int temp = currentWork.WorkIndex;
            currentWork.WorkIndex = previousWork.WorkIndex;
            previousWork.WorkIndex = temp;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Ошибка при обновлении позиций", ex);
            }
        }

        public async Task MoveDownWork(int workIndex, int categoryIndex)
        {
            var category = await _dbContext.CategoriesWorks
                .Include(c => c.Works)
                .FirstOrDefaultAsync(x => x.CategoryIndex == categoryIndex);

            if (category is null)
                throw new ArgumentException("Категория не найдена");
            var currentWork = category.Works.FirstOrDefault(x => x.WorkIndex == workIndex);
            if (currentWork == null)
                throw new ArgumentException("Работа не найдена");

            int maxIndex = category.Works.Max(w => w.WorkIndex);
            if (currentWork.WorkIndex == maxIndex)
                return;

            var nextWork = category.Works.FirstOrDefault(x => x.WorkIndex == currentWork.WorkIndex + 1);
            if (nextWork == null)
                return;

            int temp = currentWork.WorkIndex;
            currentWork.WorkIndex = nextWork.WorkIndex;
            nextWork.WorkIndex = temp;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Ошибка при обновлении позиций", ex);
            }
        }

        public async Task EditWork(ElectroWork dto, int categoryIndex)
        {
            var work = await _dbContext.ElectroWorks
                .Include(w => w.Category)
                .FirstOrDefaultAsync(w =>
                    w.WorkIndex == dto.WorkIndex &&
                    w.Category.CategoryIndex == categoryIndex);

            if (work is null) return;

            if (dto.WorkIndex != work.WorkIndex &&
                await _dbContext.ElectroWorks.AnyAsync(w =>
                    w.WorkIndex == dto.WorkIndex &&
                    w.Category.CategoryIndex == categoryIndex))
            {
                return;
            }

            work.WorkIndex = dto.WorkIndex;
            work.Name = dto.Name;
            work.Unit = dto.Unit;
            work.PriceInRubles = dto.PriceInRubles;
            work.Description = dto.Description ?? string.Empty;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<ElectroWork?> GetWork(int workIndex, int categoryIndex)
        {
            return await _dbContext.ElectroWorks
                .AsNoTracking()
                .Include(w => w.Category)
                .Where(w => w.WorkIndex == workIndex &&
                           w.Category.CategoryIndex == categoryIndex)
                .FirstOrDefaultAsync();

            //return await _dbContext.ElectroWorks
            //    .AsNoTracking()
            //    .Where(w => w.WorkIndex == workIndex)
            //    .Join(_dbContext.CategoriesWorks,
            //          work => work.CategoryId,
            //          category => category.Id,
            //          (work, category) => new { work, category })
            //    .Where(x => x.category.CategoryIndex == categoryIndex)
            //    .Select(x => x.work)
            //    .FirstOrDefaultAsync();
        }

        public async Task<List<ElectroWork>> GetAllWorksAsync()
        {
            return await _dbContext.ElectroWorks
                .AsNoTracking()
                .Include(x => x.Category)
                .ToListAsync();
        }
    }
}
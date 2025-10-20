using ElectricBusinessCard.Services.EntityFramework.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ElectricBusinessCard.Services;
using Microsoft.AspNetCore.Mvc;

namespace ElectricBusinessCard.Pages.Works
{
    public class WorksModel(
        WorkService _workService,
        CategoryService _categoryService,
        DocService _docService) : PageModel
    {
        [BindProperty]
        public List<ElectroWork> ElectroWorks { get; set; } = new();

        [BindProperty]
        public List<CategoryWork> Categories { get; set; } = new();

        public async Task<IActionResult> OnGet()
        {
            try
            {
                ElectroWorks = await _workService.GetAllWorksAsync();
                Categories = await _categoryService.GetAllCategoriesAsync();
                return Page();
            }
            catch
            {
                return Page();
            }
        }

        public IActionResult OnPostEdit(int workIndex, int categoryIndex)
        {
            return RedirectToPage("/Works/EditWorkForm", new { workIndex, categoryIndex });
        }

        public async Task<IActionResult> OnPostMove(int workIndex, int categoryIndex, bool isMoveUp)
        {
            try
            {
                if (isMoveUp is true)
                    await _workService.MoveUpWork(workIndex, categoryIndex);
                else
                    await _workService.MoveDownWork(workIndex, categoryIndex);
                return RedirectToPage();
            }
            catch
            {
                return RedirectToPage();
            }
        }

        public async Task<IActionResult> OnPostDelete(int workIndex, int categoryIndex)
        {
            try
            {
                await _workService.DeleteWorkAsync(workIndex, categoryIndex);
                return RedirectToPage();
            }
            catch
            {
                return RedirectToPage();
            }
        }
        public async Task<IActionResult> OnPostCreateCategory(string name, string description)
        {
            await _categoryService.CreateNewCategory(name, description);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteCategory(int categoryIndex)
        {
            await _categoryService.DeleteCategory(categoryIndex);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDownloadPrice()
        {
            try
            {
                var estimateExelFile = await _docService.WriteDataInSamplePrice();
                return estimateExelFile;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при создании файла: {ex.Message}");
            }
        }
    }
}

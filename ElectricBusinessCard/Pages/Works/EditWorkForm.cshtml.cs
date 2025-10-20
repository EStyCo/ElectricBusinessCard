using ElectricBusinessCard.Models;
using ElectricBusinessCard.Models.Enums;
using ElectricBusinessCard.Repository;
using ElectricBusinessCard.Services;
using ElectricBusinessCard.Services.EntityFramework.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElectricBusinessCard.Pages.Works
{
    public class EditWorkFormModel(
        WorkService _workService) : PageModel
    {
        [BindProperty]
        public List<CategoryWork> CategoriesModel  { get; set; } = new();
        [BindProperty]
        public ElectroWork WorkModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int workIndex, int categoryIndex)
        {
            var work = await _workService.GetWork(workIndex, categoryIndex);
            if (work is null) return NotFound();

            WorkModel = work;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int workIndex, int categoryIndex)
        {
            WorkModel.WorkIndex = workIndex;
            await _workService.EditWork(WorkModel, categoryIndex);

            ModelState.Remove("WorkModel.Category.Name");
            ModelState.Remove("WorkModel.Category.Works");
            if (!ModelState.IsValid) return Page();
            return RedirectToPage("/Works/Works");
        }
    }
}

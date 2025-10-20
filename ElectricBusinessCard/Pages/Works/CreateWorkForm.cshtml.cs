using ElectricBusinessCard.Models;
using ElectricBusinessCard.Repository;
using ElectricBusinessCard.Services;
using ElectricBusinessCard.Services.EntityFramework.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElectricBusinessCard.Pages.Works
{
    public class CreateWorkFormModel(
        WorkService _workService,
        CategoryService _categoryService) : PageModel
    {
        [BindProperty]
        public List<CategoryWork> CategoriesModel { get; set; } = new();
        [BindProperty]
        public ElectroWork WorkModel { get; set; } = new();
        [BindProperty]
        public int SelectedCategoryId { get; set; }
        public bool ShowMessage { get; set; }
        public bool IsSuccess { get; set; }
        public string StatusMessage { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            CategoriesModel =  await _categoryService.GetAllCategoriesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("WorkModel.Category");
            if (!ModelState.IsValid)
            {
                CategoriesModel = await _categoryService.GetAllCategoriesAsync();
                return Page();
            }

            try
            {
                await _workService.CreateElectroWorkAsync(WorkModel, SelectedCategoryId);
                StatusMessage = "������ ������� �������!";
                IsSuccess = true;
                ShowMessage = true;
                ModelState.Clear();
                CategoriesModel = await _categoryService.GetAllCategoriesAsync();
                return Page();
            }
            catch
            {
                StatusMessage = "��������� ������. ��������� ����� ��� ��������� � �������������.";
                IsSuccess = false;
                ShowMessage = true;
                return Page();
            }
        }
    }
}

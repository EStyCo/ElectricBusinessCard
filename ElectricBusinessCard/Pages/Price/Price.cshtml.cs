using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElectricBusinessCard.Pages
{
    public class PriceModel : PageModel
    {
        private readonly ILogger<PriceModel> _logger;

        public PriceModel(ILogger<PriceModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }

}

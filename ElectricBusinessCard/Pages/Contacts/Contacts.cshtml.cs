using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElectricBusinessCard.Pages.Contacts
{
    public class ContactsModel : PageModel
    {
        public void OnGet()
        {
            ViewData["ActivePage"] = "Contacts";
        }
    }
}

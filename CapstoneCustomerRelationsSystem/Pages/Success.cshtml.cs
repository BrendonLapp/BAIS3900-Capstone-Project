using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CapstoneCustomerRelationsSystem.Pages
{
    public class successModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int OrderID { get; set; }
        public void OnGet()
        {
        }
    }
}

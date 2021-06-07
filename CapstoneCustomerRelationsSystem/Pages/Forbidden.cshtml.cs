using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CapstoneCustomerRelationsSystem.Pages
{
    public class ForbiddenModel : PageModel 
    {
        public async Task<IActionResult> OnGet() {
            //Redirect to the index page.
            return RedirectToPage("/Index");
        }//end OnGet
    }//end class
}//end namespace
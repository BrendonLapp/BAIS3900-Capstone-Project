using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapstoneCustomerRelationsSystem.Domain;
using CapstoneCustomerRelationsSystem.Domain.Models;

namespace CapstoneCustomerRelationsSystem.Pages
{
    public class MyOrderDetailsModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int OrderID { get; set; }
        [BindProperty]
        public Order OrderDetails { get; set; }

        public IActionResult OnGet()
        {
            if (User.Identity.Name == null || User.Identity.Name == "")
            {
                return RedirectToPage("/Index");
            }
            else
            {
                int UserAccountNumber;
                OrdersController OrderController = new OrdersController();
                CRS Request = new CRS();
                UserAccountNumber = Request.GetUserAccountNumber(User.Identity.Name);

                OrderDetails = OrderController.GetOrderByID(OrderID);

                if (OrderDetails.CustomerUserAccountNumber == UserAccountNumber)
                {
                    return Page();
                }
                else
                {
                    return RedirectToPage("/Index");
                }
            }
        }//End OnGet

        public void OnPost()
        {

        }//End OnPost
    }
}

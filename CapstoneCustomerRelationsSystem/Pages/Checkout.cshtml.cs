using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapstoneCustomerRelationsSystem.Domain;
using CapstoneCustomerRelationsSystem.Domain.Models;

namespace CapstoneCustomerRelationsSystem.Pages
{
    public class CheckoutModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int OrderID { get; set; }
        [BindProperty]
        public string CardHolderName { get; set; }
        [BindProperty]
        public string CardNumber { get; set; }
        [BindProperty]
        public int CardExpiryMonth { get; set; }
        [BindProperty]
        public int CardExpiryYear { get; set; }
        [BindProperty]
        public string CardCVC { get; set; }
        [BindProperty]
        public string Submit { get; set; }
        public string Message { get; set; }
        public Order Order { get; set; }

        public IActionResult OnGet()
        {
            CRS Request = new CRS();
            int UserAccountNumber = Request.GetUserAccountNumber(User.Identity.Name);

            OrdersController OrderController = new OrdersController();
            Order = OrderController.GetOrderByID(OrderID);

            if (UserAccountNumber != Order.CustomerUserAccountNumber || Order.OrderStatus != "Ready For Payment")
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }//End OnGet

        public IActionResult OnPost()
        {
            OrdersController OrderController = new OrdersController();
            if (Submit == "Submit")
            {
                StripeController StripeController = new StripeController();
                Payment Payment = new Payment
                {
                    CardNumber = CardNumber,
                    CVC = CardCVC,
                    Month = CardExpiryMonth,
                    Year = CardExpiryYear
                };

                Message = StripeController.Pay(Payment, OrderID).GetAwaiter().GetResult();

                if (Message == "Success")
                {
                    return RedirectToPage("/Success");
                }
            }
            else if (Submit == "Cancel")
            {
                return Redirect("/Index");
            }
            Order = OrderController.GetOrderByID(OrderID);
            return Page();
        }//End OnPost
    }
}

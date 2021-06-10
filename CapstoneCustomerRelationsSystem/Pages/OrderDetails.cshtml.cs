using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CapstoneCustomerRelationsSystem.Domain;
using CapstoneCustomerRelationsSystem.Domain.Models;

namespace CapstoneCustomerRelationsSystem.Pages
{
    public class OrderDetailsModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int OrderID { get; set; }
        [BindProperty]
        public Order OrderDetails { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Order Status")]
        public string OrderStatus { get; set; }
        [BindProperty]
        public List<OrderItem> OrderItems { get; set; }
        [BindProperty]
        public string Submit { get; set; }

        public IActionResult OnGet()
        {
            if (User.IsInRole("Admin") || User.IsInRole("Employee") || User.IsInRole("Manager"))
            {
                int UserAccountNumber;
                OrdersController OrderController = new OrdersController();
                CRS Request = new CRS();
                UserAccountNumber = Request.GetUserAccountNumber(User.Identity.Name);

                OrderDetails = OrderController.GetOrderByID(OrderID);

                OrderItems = OrderDetails.OrderItems;
                OrderStatus = OrderDetails.OrderStatus;

                if (OrderDetails.CustomerUserAccountNumber != UserAccountNumber)
                {
                    return Page();
                }
                else
                {
                    return RedirectToPage("/Index");
                }
            }
            else
            {
                return RedirectToPage("/index");
            }
        }//End OnGet

        public IActionResult OnPost()
        {
            int UserAccountNumber;
            OrdersController OrderController = new OrdersController();
            CRS Request = new CRS();
            UserAccountNumber = Request.GetUserAccountNumber(User.Identity.Name);
            bool Success;

            if (Submit == "Update")
            {
                Success = OrderController.UpdateOrder(OrderDetails.OrderID, OrderStatus, OrderItems, UserAccountNumber);
                OnGet();
                return Page();
            }
            return Page();
        }//End OnPost
    }
}

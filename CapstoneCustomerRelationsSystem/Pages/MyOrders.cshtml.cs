using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapstoneCustomerRelationsSystem.Domain.Models;
using CapstoneCustomerRelationsSystem.Domain;

namespace CapstoneCustomerRelationsSystem.Pages
{
    public class MyOrdersModel : PageModel
    {
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 50;
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        public bool EnablePrevious => CurrentPage > 1;
        public bool EnableNext => CurrentPage < TotalPages;

        public bool ViewOrderDetails { get; set; }
        [BindProperty]
        public List<Order> Orders { get; set; }
        [BindProperty]
        public List<OrderItem> OrderItems { get; set; }
        [BindProperty]
        public string ViewOrder { get; set; }
        [BindProperty]
        public int OrderID { get; set; }
        [BindProperty]
        public int CustomerUserAccountNumber { get; set; }
        [BindProperty]
        public int EmployeeUserAccountNumber { get; set; }
        [BindProperty]
        public string OrderStatus { get; set; }
        [BindProperty]
        public decimal GST { get; set; }
        [BindProperty]
        public decimal SubTotal { get; set; }
        [BindProperty]
        public decimal Total { get; set; }
        [BindProperty]
        public string Customername { get; set; }
        [BindProperty]
        public DateTime PlacedDate { get; set; }

        public IActionResult OnGet(int currentPage)
        {
            if (User.Identity.Name == null || User.Identity.Name == "")
            {
                return RedirectToPage("/Index");
            }
            CurrentPage = currentPage == 0 ? 1 : currentPage;
            OrdersController OrdersController = new OrdersController();
            CRS Request = new CRS();
            int UserAccountNumber;
            UserAccountNumber = Request.GetUserAccountNumber(User.Identity.Name);
            Orders = OrdersController.GetOrdersByUserAccountNumber(UserAccountNumber);

            Count = Orders.Count();

            if (CurrentPage > TotalPages)
            {
                CurrentPage = TotalPages;
            }

            Orders = Orders
                    .Skip((CurrentPage - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();

            return Page();
        }//End OnGet
    }
}

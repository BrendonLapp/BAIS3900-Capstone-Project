using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using CapstoneCustomerRelationsSystem.Domain;
using CapstoneCustomerRelationsSystem.Domain.Models;

namespace CapstoneCustomerRelationsSystem.Pages
{
    [Authorize(Roles = "Admin, Employee, Manager")]
    public class ViewOrdersModel : PageModel
    {
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 25;
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        public bool EnablePrevious => CurrentPage > 1;
        public bool EnableNext => CurrentPage < TotalPages;
        public bool ViewOrderDetails { get; set; }
        [BindProperty]
        public string SearchQuery { get; set; }
        [BindProperty]
        public string Submit { get; set; }
        [BindProperty]
        public string OrderStatus { get; set; }
        public List<Order> Orders { get; set; }
       
        public IActionResult OnGet(int currentPage)
        {
            if (User.IsInRole("Admin") || User.IsInRole("Employee") || User.IsInRole("Manager"))
            {
                CurrentPage = currentPage == 0 ? 1 : currentPage;
                OrdersController OrdersController = new OrdersController();

                Orders = OrdersController.GetIncompleteOrders();
                Orders = PageOrders(Orders);
                return Page();
            }
            else
            {
                return RedirectToPage("/Index");
            }
        }//End OnGet

        public IActionResult OnPost()
        {
            OrdersController OrdersController = new OrdersController();
            Order FoundOrder = new Order();
            if (Submit == "Search")
            {
                if (SearchQuery != null && SearchQuery != "")
                {
                    if (int.TryParse(SearchQuery, out int SearchID))
                    {
                        FoundOrder = OrdersController.GetOrderByID(SearchID);

                        if (FoundOrder != null)
                        {
                            return Redirect("~/OrderDetails/" + SearchID);
                        }
                        else
                        {
                            return Page();
                        }
                    }
                    return Page();
                }
                else if (OrderStatus != "Select a Status")
                {
                    Orders = OrdersController.GetOrdersByStatus(OrderStatus);
                    Orders = PageOrders(Orders);
                    return Page();
                }
                return Page();
            }
            return Page();
        }//End OnPost

        public List<Order> PageOrders(List<Order> Orders)
        {
            Count = Orders.Count();

            if (CurrentPage > TotalPages)
            {
                CurrentPage = TotalPages;
            }

            Orders = Orders
                        .Skip((CurrentPage - 1) * PageSize)
                        .Take(PageSize)
                        .ToList();

            return Orders;
        }//End PageOrders
    }
}

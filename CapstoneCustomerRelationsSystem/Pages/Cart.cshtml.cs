using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using CapstoneCustomerRelationsSystem.Domain;
using CapstoneCustomerRelationsSystem.Domain.Models;

namespace CapstoneCustomerRelationsSystem.Pages
{
    
    public class CartModel : PageModel
    {
        [BindProperty]
        public string Submit { get; set; }
        [BindProperty]
        public string Delete { get; set; }
        [BindProperty]
        public int CartID { get; set; }
        [BindProperty]
        public List<Cart> Cart { get; set; }
        [BindProperty]
        public List<int> Quantity { get; set; }
        [BindProperty]
        public decimal Total { get; set; }
        [BindProperty]
        public decimal SubTotal { get; set; }
        [BindProperty]
        public decimal GST { get; set; }
        [BindProperty]
        public string CapstoneLocation { get; set; }
        public string Message { get; set; }
        public string PickupMessage { get; set; }
        public IActionResult OnGet()
        {
            if (User.Identity.Name == null || User.Identity.Name == "")
            {
                return RedirectToPage("/Index");
            }
            StripeController StripeController = new StripeController();
            decimal TaxRate = StripeController.GetProvincalTaxRate("AB");
            
            CRS Request = new CRS();
            int UserAccountNumber;
            UserAccountNumber = Request.GetUserAccountNumber(User.Identity.Name);

            CartController CartController = new CartController();
            Cart = CartController.GetCart(UserAccountNumber);

            foreach (var Item in Cart)
            {
                SubTotal += Item.Price;
            }
            GST = SubTotal * decimal.Round(TaxRate, 2);
            Total = decimal.Round(SubTotal, 2) + decimal.Round(GST, 2);
            return Page();
        }//End OnGet

        public IActionResult OnPost()
        {
            CRS Request = new CRS();
            CartController CartController = new CartController();
            OrdersController OrderController = new OrdersController();
            bool Success;
            int UserAccountNumber;
            UserAccountNumber = Request.GetUserAccountNumber(User.Identity.Name);


            if (Submit == "Request Order")
            {
                if (Total < 0.50M)
                {
                    Message = "Cannot order less than 0.50c of product.";
                    Cart = CartController.GetCart(UserAccountNumber);
                    return Page();
                }

                if (CapstoneLocation == null || CapstoneLocation == "")
                {
                    PickupMessage = "You must select a Capstone Comics and Games location for pickup.";
                    Cart = CartController.GetCart(UserAccountNumber);
                    return Page();
                }

                Total = 0.00M;
                Cart = CartController.GetCart(UserAccountNumber);

                StripeController StripeController = new StripeController();
                decimal TaxRate = StripeController.GetProvincalTaxRate("AB");
                List<OrderItem> OrderItems = new List<OrderItem>();

                foreach (var Item in Cart)
                {
                    OrderItem NewItem = new OrderItem();
                    if (int.TryParse(Item.ID, out int ID))
                    {
                        NewItem.ProductID = ID;
                    }
                    else
                    {
                        NewItem.CardID = Item.ID;
                    }

                    NewItem.Name = Item.Name;
                    NewItem.QuantityRequested = Item.Quantity;
                    NewItem.LineItemPrice = Item.Price;
                    SubTotal = SubTotal + Item.Price;
                    OrderItems.Add(NewItem);
                }

                GST = SubTotal * 0.05M;
                Total = SubTotal + GST;

                Order NewOrder = new Order
                {
                    CustomerUserAccountNumber = UserAccountNumber,
                    PlacedDate = DateTime.Now,
                    OrderStatus = "Pending",
                    OrderItems = OrderItems,
                    GST = decimal.Round(GST, 2),
                    SubTotal = decimal.Round(SubTotal, 2),
                    Total = decimal.Round(Total, 2),
                    CapstoneLocation = CapstoneLocation
                };

                OrderController.AddOrder(NewOrder, UserAccountNumber);
                Message = "Thank you for placing your order with Capstone. We will let you know which items are currently in stock.";
                return Page();
            }

            if (Submit == "Clear Cart")
            {
                Success = CartController.DeleteCart(UserAccountNumber);
                Message = "You cart has been cleared";
                return Page();
            }

            if (Submit == "Update")
            {
                Success = CartController.UpdateCart(Cart);
                Message = "Successfully updated your cart.";
                return Page();
            }
            return Page();
        }//End OnPost

        public void OnPostDelete(int CartID)
        {
            if (Delete == "Delete From Cart")
            {
                bool Success;
                CartController CartController = new CartController();
                Success = CartController.DeleteFromCart(CartID);

                OnGet();
            }
        }//OnPostDelete
    }
}

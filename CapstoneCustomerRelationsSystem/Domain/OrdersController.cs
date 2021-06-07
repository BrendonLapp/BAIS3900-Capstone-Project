using System;
using System.Collections.Generic;
using CapstoneCustomerRelationsSystem.Domain.Models;
using CapstoneCustomerRelationsSystem.TechnicalServices;

namespace CapstoneCustomerRelationsSystem.Domain
{
    public class OrdersController
    {
        public List<Order> GetIncompleteOrders()
        {
            OrderManager OrderManager = new OrderManager();
            List<Order> Orders = new List<Order>();

            try
            {
                Orders = OrderManager.GetIncompletedOrders();
            }
            catch
            {
                Orders = null;
            }

            return Orders;
        }//End GetIncompleteOrders

        public Order GetOrderByID(int OrderID)
        {
            OrderManager OrderManager = new OrderManager();
            Order FoundOrder = new Order();
            CRS Request = new CRS();
            MTGCardController CardController = new MTGCardController();
            StripeController StripeController = new StripeController();
            decimal TaxRate = StripeController.GetProvincalTaxRate("AB");

            try
            {
                FoundOrder = OrderManager.GetOrdersByOrderID(OrderID);
                FoundOrder.OrderItems = OrderManager.FindOrderItems(OrderID);

                foreach (var Item in FoundOrder.OrderItems)
                {
                    if (int.TryParse(Item.SharedID, out int value))
                    {
                        Product FoundProduct = new Product();
                        FoundProduct = Request.RetrieveProduct(value);
                        Item.ProductID = FoundProduct.ProductNumber;
                        Item.Name = FoundProduct.ProductName;
                        Item.Image = FoundProduct.ImageURL;
                    }
                    else
                    {
                        Card FoundCard = new Card();
                        FoundCard = CardController.GetCard(Item.SharedID);
                        Item.CardID = FoundCard.Id;
                        Item.Name = FoundCard.Name;
                        Item.ExtraIdentifier = FoundCard.Set + " " + FoundCard.Collector_Number;
                        Item.Image = FoundCard.ImageSmall;
                    }
                    //FoundOrder.SubTotal = (Item.LineItemPrice + FoundOrder.SubTotal);
                }


                FoundOrder.GST = FoundOrder.SubTotal * TaxRate;
                FoundOrder.Total = FoundOrder.SubTotal + FoundOrder.GST;

                decimal.Round(FoundOrder.GST, 2);
                decimal.Round(FoundOrder.Total, 2);
                decimal.Round(FoundOrder.SubTotal, 2);
            }
            catch
            {
                FoundOrder = null;
            }

            return FoundOrder;
        }//End GetOrderByID

        public List<Order> GetOrdersByStatus(string Status)
        {
            OrderManager OrderManager = new OrderManager();
            List<Order> Orders = new List<Order>();

            try
            {
                Orders = OrderManager.GetOrdersByStatus(Status);
            }
            catch
            {
                Orders = null;
            }

            return Orders;
        }//End GetOrdersByStatus

        public bool AddOrder(Order NewOrder, int UserAccountNumber)
        {
            bool Confirmation;
            int OrderID;
            OrderManager OrderManager = new OrderManager();
            MTGCardController CardController = new MTGCardController();
            CartController CartController = new CartController();
            ContactController ContactController = new ContactController();
            CRS Request = new CRS();
            try
            {
                OrderID = OrderManager.AddOrder(NewOrder);
                NewOrder.OrderID = OrderID;
                foreach (var Item in NewOrder.OrderItems)
                {
                    OrderItem OrderItem = new OrderItem
                    {
                        OrderID = OrderID,
                        QuantityRequested = Item.QuantityRequested,
                        LineItemPrice = Item.LineItemPrice
                    };

                    if (Item.CardID != null)
                    {
                        Card Card = new Card();
                        Card = CardController.GetCard(Item.CardID);
                        OrderItem.LineItemPrice = Card.Price;
                        OrderItem.CardID = Item.CardID;
                        OrderManager.AddCardOrderItem(OrderItem);
                    }
                    else
                    {
                        Product Product = new Product();
                        Product = Request.RetrieveProduct(Item.ProductID);
                        OrderItem.LineItemPrice = Product.ProductPrice;
                        OrderItem.ProductID = Item.ProductID;
                        OrderManager.AddProductOrderItem(OrderItem);
                    }

                }

                CartController.DeleteCart(UserAccountNumber);

                string Subject = "Capstone Order: #" + OrderID + " has been placed";
                string Text = "Your order with Capstone Comics and games has been placed. " +
                    "Please contact us at (780) 433-7119 for Capstone 1, (780) 478-7767 for Capstone 2, (780) 462-5767 for Capstone 3 if you have any questions regarding your order.";
                string HtmlText = "Your order with Capstone Comics and games has been placed. Plase visit the My Orders page to view when your order is ready for purchase." + "<br/>" +
                    "Please contact us at (780) 433-7119 for Capstone 1, (780) 478-7767 for Capstone 2, (780) 462-5767 for Capstone 3 if you have any questions regarding your order.";

                ContactController.SendEmailForOrder(NewOrder, Subject, Text, HtmlText);
                Confirmation = true;
            }
            catch
            {
                Confirmation = false;
            }

            return Confirmation;
        }//End AddOrder

        public bool UpdateOrder(int OrderID, string OrderStatus, List<OrderItem> OrderItems, int UserAccountNumber)
        {
            bool Confirmation;
            bool ItemConfirmation = false;
            decimal GST;
            decimal SubTotal = 0;
            decimal Total;
            OrderManager OrderManager = new OrderManager();
            ContactController ContactController = new ContactController();
            StripeController StripeController = new StripeController();
            decimal TaxRate = StripeController.GetProvincalTaxRate("AB");
            Order ExistingOrderDetails = new Order();
            ExistingOrderDetails = GetOrderByID(OrderID);

            foreach (var Item in ExistingOrderDetails.OrderItems)
            {
                foreach (var OrderItem in OrderItems)
                {
                    if (Item.OrderItemID == OrderItem.OrderItemID)
                    {
                        OrderItem.LineItemPrice = Item.LineItemPrice;
                    }
                }
            }

            foreach (var Item in OrderItems)
            {
                SubTotal = SubTotal + ((decimal)Item.QuantityOnHand * Item.LineItemPrice);
            }

            GST = SubTotal * TaxRate;
            Total = SubTotal + GST;

            Order OrderDetails = new Order
            {
                OrderID = OrderID,
                GST = decimal.Round(GST, 2),
                SubTotal = decimal.Round(SubTotal, 2),
                Total = decimal.Round(Total, 2),
                OrderStatus = OrderStatus,
                EmployeeUserAccountNumber = UserAccountNumber
            };

            try
            {
                if (OrderStatus == "Complete")
                {
                    OrderDetails.CompletedDate = DateTime.Now;
                    Confirmation = OrderManager.CompleteOrder(OrderDetails);
                    Order CustomerOrder = new Order();

                    string Subject = "Thank you for your purchase at Capstone Comics and Games - Receipt for Order: " + OrderID;
                    string Text = "Thank you for your purchase at Capstone Comics and Games. Your order will be ready for pick up. Please contact a Capstone store for your pickup location";
                    string HtmlText = "Thank you for your purchase at Capstone Comics and Games. Your order will be ready for pick up. Please contact a Capstone store for your pickup location."
                    + "<br/>" +
                    "Please contact us at (780) 433-7119 for Capstone 1, (780) 478-7767 for Capstone 2, (780) 462-5767 for Capstone 3 for questions regarding your order.";

                    CustomerOrder = GetOrderByID(OrderID);
                    ContactController.SendEmailForOrder(CustomerOrder, Subject, Text, HtmlText);
                }
                else
                {
                    Confirmation = OrderManager.UpdateOrder(OrderDetails);
                }

                if (Confirmation == true)
                {
                    foreach (var Item in OrderItems)
                    {
                        OrderManager.UpdateOrderItem(Item);
                    }
                    ItemConfirmation = true;
                }

                if (OrderStatus == "Ready For Payment")
                {
                    Order CustomerOrder = new Order();

                    string Subject = "Your order is ready for purchase - Capstone Comics and Games - Order: " + OrderID;
                    string Text = "Your order: " + OrderID + " is ready for purchase. Please visit the My Orders page to purchase your order, or in store at your chosen Capstone Comics and Games location.";
                    string HtmlText = "Your order: " + OrderID + " is ready for purchase. Please visit the My Orders page to purchase your order, or in store at your chosen Capstone Comics and Games location."
                    + "<br/>" +
                    "Please contact us at (780) 433-7119 for Capstone 1, (780) 478-7767 for Capstone 2, (780) 462-5767 for Capstone 3 for questions regarding your order.";

                    CustomerOrder = GetOrderByID(OrderID);
                    ContactController.SendEmailForOrder(CustomerOrder, Subject, Text, HtmlText);
                }
            }
            catch
            {
                Confirmation = false;
            }

            if (ItemConfirmation == true && Confirmation == true)
            {
                return Confirmation;
            }
            else
            {
                return false;
            }
        }//End UpdateOrder

        public bool UpdateOrder(Order Order, string OrderStatus)
        {
            bool Confirmation;
            //decimal GST;
            //decimal SubTotal = 0;
            //decimal Total;

            OrderManager OrderManager = new OrderManager();

            try
            {
                Confirmation = OrderManager.UpdateOrder(Order, OrderStatus, DateTime.Now);
            }
            catch
            {
                Confirmation = false;
            }

            if (Confirmation == true)
            {
                return Confirmation;
            }
            else
            {
                return false;
            }
        }//End UpdateOrder

        public List<Order> GetOrdersByUserAccountNumber(int UserAccountNumber)
        {
            List<Order> CustomerOrders = new List<Order>();
            OrderManager OrderManager = new OrderManager();

            
            try
            {
                CustomerOrders = OrderManager.GetOrdersByUserAccountNumber(UserAccountNumber);
            }
            catch
            {
                CustomerOrders = null;
            }

            return CustomerOrders;
        }//End GetOrdersByUserAccountNumber

    }//End Class
}

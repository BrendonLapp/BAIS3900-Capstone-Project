using Stripe;
using System;
using System.Linq;
using System.Threading.Tasks;
using CapstoneCustomerRelationsSystem.Domain.Models;
using CapstoneCustomerRelationsSystem.TechnicalServices;

namespace CapstoneCustomerRelationsSystem.Domain
{
    public class StripeController
    {
        public async Task<string> Pay(Payment Payment, int OrderID)
        {
            int ConvertedAmount;
            string Message;
            bool Success = true;
            OrdersController OrderController = new OrdersController();
            ContactController ContactController = new ContactController();
            StripeManager stripeManager = new StripeManager();
            Models.Order CustomerOrder = new Models.Order();

            CustomerOrder = GetOrderAmount(OrderID);

            ConvertedAmount = GetPaymentAmount(CustomerOrder.Total);

            Message = await stripeManager.PayAsync(Payment.CardNumber, Payment.Month, Payment.Year, Payment.CVC, ConvertedAmount);

            if (Message == "Success")
            {
                Success = OrderController.UpdateOrder(CustomerOrder, "Completed");
                if (Success == true)
                {
                    string Subject = "Thank you for your purchase at Capstone Comics and Games - Receipt for Order: " + OrderID;
                    string Text = "Thank you for your purchase at Capstone Comics and Games. Your order will be ready for pick up. Please contact a Capstone store for your pickup location";
                    string HtmlText = "Thank you for your purchase at Capstone Comics and Games. Your order will be ready for pick up. Please contact a Capstone store for your pickup location."
                    + "<br/>" +
                    "Please contact us at (780) 433-7119 for Capstone 1, (780) 478-7767 for Capstone 2, (780) 462-5767 for Capstone 3 for questions regarding your order.";

                    CustomerOrder = OrderController.GetOrderByID(OrderID);
                    ContactController.SendEmailForOrder(CustomerOrder, Subject, Text, HtmlText);
                }
            }

            if (Success == false)
            {
                Message = "Something has gone wrong. Please contact Capstone Comics and Games for assistance. Your Order Number is: " + OrderID;
            }

            return Message;
        }//End Pay

        public decimal GetProvincalTaxRate(string Province)
        {
            StripeList<TaxRate> taxRates = new StripeList<TaxRate>();
            taxRates = GetTaxRates();
            decimal ProvincialTaxRate;

            ProvincialTaxRate = taxRates.Where(x => x.Country == "CA" 
                                                && x.State == Province 
                                                && x.Active == true)
                                        .Select(x => x.Percentage)
                                        .FirstOrDefault();

            ProvincialTaxRate = ProvincialTaxRate / 100;

            return ProvincialTaxRate;
        }//End GetProvincalTaxRate

        internal StripeList<TaxRate> GetTaxRates()
        {
            StripeList<TaxRate> taxRates = new StripeList<TaxRate>();
            StripeManager StripeManager = new StripeManager();

            taxRates = StripeManager.GetTaxRates();

            return taxRates;
        }//End GetTaxRates

        internal Models.Order GetOrderAmount(int OrderID)
        {
            decimal OrderValue;
            OrdersController OrderController = new OrdersController();
            Models.Order CustomerOrder = new Models.Order();
            CustomerOrder = OrderController.GetOrderByID(OrderID);

            decimal TaxRate = GetProvincalTaxRate("AB");

            OrderValue = CustomerOrder.SubTotal * TaxRate;

            Models.Order OrderToUpdate = new Models.Order()
            {
                OrderID = OrderID,
                GST = TaxRate * CustomerOrder.SubTotal,
                Total = (TaxRate * CustomerOrder.SubTotal) + CustomerOrder.SubTotal
            };
            return OrderToUpdate;
        }//End GetOrderAmount

        internal int GetPaymentAmount(decimal Value)
        {
            int Amount;

            //Amount = Convert.ToInt32(Value);
            decimal beforeConvert = Value * 100M;
            Amount = Convert.ToInt32(beforeConvert);

            return Amount;
        }//End GetPaymentAmount
    }
}

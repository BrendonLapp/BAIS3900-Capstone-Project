using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CapstoneCustomerRelationsSystem.Domain.Models
{
    public class Order
    {
        [Display(Name = "Order ID")]
        public int OrderID { get; set; }
        [Display(Name = "Customer Account Number")]
        public int CustomerUserAccountNumber { get; set; }
        [Display(Name = "Employee Account Number")]
        public int EmployeeUserAccountNumber { get; set; }
        [Display(Name = "Placed Date")]
        public DateTime PlacedDate { get; set; }
        [Display(Name = "Completed Date")]
        public DateTime CompletedDate { get; set; }
        [Display(Name = "Order Status")]
        public string OrderStatus { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public decimal GST { get; set; }
        [Display(Name = "Sub Total")]
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
        [Display(Name = "Capstone Location")]
        public string CapstoneLocation { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }//End Class
}

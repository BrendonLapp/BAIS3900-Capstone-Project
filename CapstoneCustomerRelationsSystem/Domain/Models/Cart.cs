using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneCustomerRelationsSystem.Domain.Models
{
    public class Cart
    {
        public int CartID { get; set; }
        public int UserAccountNumber { get; set; }
        public string ID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ImageLink { get; set; }
        public string Name { get; set; }
    }
}

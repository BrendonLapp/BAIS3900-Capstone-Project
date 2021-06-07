namespace CapstoneCustomerRelationsSystem.Domain.Models
{
    public class OrderItem
    {
        public int OrderItemID { get; set; }
        public int OrderID { get; set; }
        public string SharedID { get; set; }
        public int ProductID { get; set; }
        public string CardID { get; set; }
        public int QuantityRequested { get; set; }
        public int? QuantityOnHand { get; set; }
        public decimal LineItemPrice { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string ExtraIdentifier { get; set; }
    }//End Class
}

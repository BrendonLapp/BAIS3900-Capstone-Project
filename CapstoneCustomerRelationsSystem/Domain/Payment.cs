namespace CapstoneCustomerRelationsSystem.Domain
{
    public class Payment
    {
        public string CardNumber { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string CVC { get; set; }
        public decimal Value { get; set; }
    }
}

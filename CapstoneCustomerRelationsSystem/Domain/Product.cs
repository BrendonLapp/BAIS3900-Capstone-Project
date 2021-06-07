namespace CapstoneCustomerRelationsSystem.Domain 
{
    public class Product 
    {
        public int ProductNumber { get; set; }
        public string ImageURL { get; set; }
        // public string ProductLink { get; set; }

        //ToDo: Add other properties for search terms.
        public string CompanyName { get; set; }

        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }

        public Product() {

        }//end Constructor
    }//end class
}//end namespace
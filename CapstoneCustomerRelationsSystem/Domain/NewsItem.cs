using System.Collections.Generic;

namespace CapstoneCustomerRelationsSystem.Domain 
{
    public class NewsItem 
    {
        public int NewsItemNumber { get; set; }
        public string ImageType { get; set; }
        public string ImageURL { get; set; }
        public string NewsItemLink { get; set; }
        public int IndexPosition { get; set; }

        public string OptionalNewsName { get; set; }
        public string OptionalNewsDescription { get; set; }
        public string OptionalNewsPrice { get; set; }

        public NewsItem() {

        }//end Constructor
        
    }//end class
}//end namespace
using System;

namespace CapstoneCustomerRelationsSystem.Domain
{
    public class Card
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
        public string Set { get; set; }
        public string SetName { get; set; }
        public string Collector_Number { get; set; }
        public string Rarity { get; set; }
        public string ImageSmall { get; set; }
        public string ImageNormal { get; set; }
        public string ImageLarge { get; set; }
        public decimal Price { get; set; }
        public DateTime LastUpdate { get; set; }
    }//End Class
}

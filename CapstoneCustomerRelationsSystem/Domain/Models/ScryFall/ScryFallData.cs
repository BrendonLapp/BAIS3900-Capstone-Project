using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneCustomerRelationsSystem.Domain.Models.ScryFall
{
    public class ScryFallData
    {
        public string id { get; set; }
        public string name { get; set; }
        public string uri { get; set; }
        public ImageUriRoot image_uris { get; set; }
        public string set { get; set; }
        public string collector_number { get; set; }
        public string rarity { get; set; }
        public List<CardFaces> card_faces { get; set; }
        public PriceRoot prices { get; set; }
    }//End Class
}

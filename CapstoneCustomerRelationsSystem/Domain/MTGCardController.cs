using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CapstoneCustomerRelationsSystem.Domain.Models.ScryFall;
using CapstoneCustomerRelationsSystem.Domain.Models.Valet;
using CapstoneCustomerRelationsSystem.TechnicalServices;
using System.Threading;

namespace CapstoneCustomerRelationsSystem.Domain
{
    public class MTGCardController
    {    
        public bool AddCardsToDatabase()
        {
            bool Confirmation = false;
            ScryFallController ScryFallController = new ScryFallController();
            MTGCardManager MTGCardManager = new MTGCardManager();
            SetsRoot SetRoot = new SetsRoot();
            List<string> ExistingSets = new List<string>();

            decimal ConversionRate = GetCanadianConversion().GetAwaiter().GetResult();

            SetRoot = ScryFallController.GetSets();
            ExistingSets = MTGCardManager.GetExistingSets();

            HashSet<string> SetCodes = new HashSet<string>(ExistingSets.Select(x => x));
            SetRoot.data.RemoveAll(x => SetCodes.Contains(x.code));

            foreach (var Set in SetRoot.data)
            {
                ScryFallRoot ScryFallRoot = new ScryFallRoot();
                ScryFallRoot = ScryFallController.GetScryFallBySet(Set.code);

                if (ScryFallRoot.data != null)
                {
                    foreach (var Card in ScryFallRoot.data)
                    {
                        Card NewCard = new Card();
                        NewCard.Id = Card.id;
                        NewCard.Collector_Number = Card.collector_number;
                        NewCard.Name = Card.name;
                        NewCard.Uri = Card.uri;
                        NewCard.Rarity = Card.rarity;
                        if (Card.prices.usd != null)
                        {
                            NewCard.Price = decimal.Parse(Card.prices.usd);
                        }
                        else if (Card.prices.usd == null)
                        {
                            if (Card.prices.usd_foil != null)
                            {
                                NewCard.Price = decimal.Parse(Card.prices.usd_foil);
                            }
                            else
                            {
                                NewCard.Price = 0.00M;
                            }
                        }
                        NewCard.Set = Card.set;
                        NewCard.SetName = Set.name;
                        if (Card.image_uris == null)
                        {
                            NewCard.ImageSmall = Card.card_faces.Select(x => x.image_uris.small).FirstOrDefault().ToString();
                            NewCard.ImageNormal = Card.card_faces.Select(x => x.image_uris.normal).FirstOrDefault().ToString();
                            NewCard.ImageLarge = Card.card_faces.Select(x => x.image_uris.large).FirstOrDefault().ToString();
                        }
                        else
                        {
                            NewCard.ImageSmall = Card.image_uris.small;
                            NewCard.ImageNormal = Card.image_uris.normal;
                            NewCard.ImageLarge = Card.image_uris.large;
                        }
                        try
                        {
                            MTGCardManager.AddMTGCard(NewCard);
                        }
                        catch
                        {
                            Confirmation = false;
                        }
                    }
                }
                Confirmation = true;
            }
            return Confirmation;
        }//End AddCardsToDatabase();

        public bool UpdateCardPrices()
        {
            bool Confirmation = false;
            ScryFallController ScryFallController = new ScryFallController();
            MTGCardManager MTGCardManager = new MTGCardManager();
            SetsRoot SetRoot = new SetsRoot();
            List<string> ExistingSets = new List<string>();

            decimal ConversionRate = GetCanadianConversion().GetAwaiter().GetResult();

            SetRoot = ScryFallController.GetSets();
            ExistingSets = MTGCardManager.GetExistingSets();

            foreach (var Set in SetRoot.data)
            {
                ScryFallRoot ScryFallRoot = new ScryFallRoot();
                Thread.Sleep(1000);
                ScryFallRoot = ScryFallController.GetScryFallBySet(Set.code);
                
                if (ScryFallRoot.data != null)
                {
                    foreach (var Card in ScryFallRoot.data)
                    {
                        string CardID = Card.id;
                        decimal Price = 0.00M;
                        if (Card.prices.usd != null)
                        {
                            Price = decimal.Parse(Card.prices.usd);
                        }
                        else if (Card.prices.usd == null)
                        {
                            if (Card.prices.usd_foil != null)
                            {
                                Price = decimal.Parse(Card.prices.usd_foil);
                            }
                            else
                            {
                                Price = 0.00M;
                            }
                        }

                        MTGCardManager.UpdateCardPrice(CardID, Price);
                    }
                    Confirmation = true;
                }
                else
                {
                    Confirmation = false;
                }
            }
            return Confirmation;
        }//End UpdateCardPrices

        public List<Card> GetCards()
        {
            List<Card> CardList = new List<Card>();
            MTGCardManager MTGCardManager = new MTGCardManager();

            CardList = MTGCardManager.GetAllCards();

            return CardList;
        }//End GetCards

        public List<Card> GetCards(string Query)
        {
            List<Card> CardList = new List<Card>();

            MTGCardManager MTGCardManager = new MTGCardManager();

            CardList = MTGCardManager.GetAllCardsByQuery(Query);

            return CardList;
        }//End GetCards

        public Card GetCard(string CardID)
        {
            MTGCardManager MTGCardManager = new MTGCardManager();
            Card MTGCard = new Card();

            MTGCard = MTGCardManager.GetCard(CardID);

            return MTGCard;
        }//End GetCard

        private async Task<decimal> GetCanadianConversion()
        {
            decimal ConversionRate;
            ValetRoot ValetRoot = new ValetRoot();

            using (var HttpClient = new HttpClient())
            {
                using (var Response = await HttpClient.GetAsync("https://www.bankofcanada.ca/valet/observations/FXUSDCAD/json?recent=1"))
                {
                    string ApiResponse = await Response.Content.ReadAsStringAsync();
                    ValetRoot = JsonConvert.DeserializeObject<ValetRoot>(ApiResponse);
                }
            }

            ConversionRate = decimal.Parse(ValetRoot.observations.Select(x => x.FXUSDCAD.v).FirstOrDefault());

            return ConversionRate;
        }//End GetCandianConversion
    }//End MTGCardController
}

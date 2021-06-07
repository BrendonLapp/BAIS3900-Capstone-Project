using System.Collections.Generic;
using System.Linq;
using CapstoneCustomerRelationsSystem.Domain.Models;
using CapstoneCustomerRelationsSystem.TechnicalServices;

namespace CapstoneCustomerRelationsSystem.Domain
{
    public class BuylistController
    {
        public bool AddToBuylist(string CardID)
        {
            bool Confirmation;

            BuylistManager BuylistManager = new BuylistManager();

            List<BuylistCard> ExistingBuylist = new List<BuylistCard>();
            ExistingBuylist = BuylistManager.GetBuylist();
            HashSet<string> CardsOnBuylist = new HashSet<string>(ExistingBuylist.Select(x => x.Id));

            if (CardsOnBuylist.Contains(CardID))
            {
                Confirmation = false;
            }
            else
            {
                try
                {
                    Confirmation = BuylistManager.AddToBuylist(CardID);
                }
                catch
                {
                    Confirmation = false;
                }
            }

            return Confirmation;
        }//End AddToBuylist

        public List<BuylistCard> GetBuylist()
        {
            BuylistManager BuylistController = new BuylistManager();
            List<BuylistCard> Buylist = new List<BuylistCard>();

            try
            {
                Buylist = BuylistController.GetBuylist();
            }
            catch
            {
                Buylist = null;
            }
            
            return Buylist;
        }//End GetBuylist

        public List<BuylistCard> GetBuylist(string Query)
        {
            BuylistManager BuylistController = new BuylistManager();
            List<BuylistCard> Buylist = new List<BuylistCard>();

            try
            {
                Buylist = BuylistController.GetBuylist(Query);
            }
            catch
            {
                Buylist = null;
            }

            return Buylist;
        }//End GetBuylist

        public bool DeleteFromBuylist(int BuylistID)
        {
            bool Confirmation;

            BuylistManager BuylistController = new BuylistManager();

            Confirmation = BuylistController.DeleteFromBuylist(BuylistID);

            return Confirmation;
        }//End DeleteFromBuylist

        public List<Card> GetCards()
        {
            List<Card> CardList = new List<Card>();
            List<BuylistCard> ExistingBuylist = new List<BuylistCard>();
            MTGCardManager MTGCardManager = new MTGCardManager();

            ExistingBuylist = GetBuylist();
            HashSet<string> CardsOnBuylist = new HashSet<string>(ExistingBuylist.Select(x => x.Id));

            try
            {
                CardList = MTGCardManager.GetAllCards();
                CardList.RemoveAll(x => CardsOnBuylist.Contains(x.Id));
            }
            catch
            {
                CardList = null;
            }
            return CardList;
        }//End GetCards

        public List<Card> GetCards(string Query)
        {
            List<Card> CardList = new List<Card>();
            List<BuylistCard> ExistingBuylist = new List<BuylistCard>();
            MTGCardManager MTGCardManager = new MTGCardManager();

            ExistingBuylist = GetBuylist();
            HashSet<string> CardsOnBuylist = new HashSet<string>(ExistingBuylist.Select(x => x.Id));

            try
            {
                CardList = MTGCardManager.GetAllCardsByQuery(Query);
            }
            catch
            {
                CardList = null;
            }

            return CardList;
        }//End GetCards
    }
}

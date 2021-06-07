using CapstoneCustomerRelationsSystem.Domain.Models.ScryFall;
using CapstoneCustomerRelationsSystem.TechnicalServices;

namespace CapstoneCustomerRelationsSystem.Domain
{
    public class ScryFallController
    {
        public ScryFallRoot GetScryfallCards()
        {

            ScryfallManager ScryfallManager = new ScryfallManager();
            ScryFallRoot Scryfall = new ScryFallRoot();

            Scryfall = ScryfallManager.GetCardsFromScryFall().GetAwaiter().GetResult();

            return Scryfall;
        }//End GetScryfallCards

        public ScryFallRoot GetScryFallCards(string PageUrl)
        {
            ScryfallManager ScryFallManager = new ScryfallManager();
            ScryFallRoot ScryFall = new ScryFallRoot();

            ScryFall = ScryFallManager.GetCardsFromScryFall(PageUrl).GetAwaiter().GetResult();

            return ScryFall;
        }//End GetScryFallCards

        public SetsRoot GetSets()
        {
            ScryfallManager ScryFallManager = new ScryfallManager();
            SetsRoot SetsRoot = new SetsRoot();

            SetsRoot = ScryFallManager.GetAllSets().GetAwaiter().GetResult();

            return SetsRoot;
        }//End GetSets

        public ScryFallRoot GetScryFallBySet(string SetCode)
        {
            ScryfallManager ScryFallManager = new ScryfallManager();
            ScryFallRoot ScryFallRoot = new ScryFallRoot();

            ScryFallRoot = ScryFallManager.GetCardsInSet(SetCode).GetAwaiter().GetResult();

            return ScryFallRoot;
        }//End GetScryFallBySet
    }
}

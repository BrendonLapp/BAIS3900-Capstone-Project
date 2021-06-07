using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapstoneCustomerRelationsSystem.Domain;

namespace CapstoneCustomerRelationsSystem.Pages
{
    public class AddToBuylistModel : PageModel
    {
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 50;
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        public bool EnablePrevious => CurrentPage > 1;
        public bool EnableNext => CurrentPage < TotalPages;
        [BindProperty]
        public string start { get; set; }
        [BindProperty]
        public string Button { get; set; }
        [BindProperty]
        public string ID { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }
        [BindProperty]
        public List<Card> CardData { get; set; }
        public string Message { get; set; }
        public IActionResult OnGet(int currentPage)
        {
            if (User.IsInRole("Admin"))
            {
                CurrentPage = currentPage == 0 ? 1 : currentPage;
                BuylistController BuylistController = new BuylistController();
                List<Card> CardList = new List<Card>();
                CardList = BuylistController.GetCards();

                CardData = CardList;
                if (SearchQuery != null && SearchQuery != "")
                {
                    CardData = BuylistController.GetCards(SearchQuery);
                    CardData = PageCards(CardData);
                }
                else
                {
                    CardData = BuylistController.GetCards();
                    CardData = PageCards(CardData);
                }
                return Page();
            }
            else
            {
                return Redirect("~/Forbidden");
            }
        }//End OnGet

        public void OnPost(int currentPage)
        {
            CurrentPage = currentPage == 0 ? 1 : currentPage;
            if (Button == "Search")
            {
                if (SearchQuery != null && SearchQuery != "")
                {
                    BuylistController BuylistController = new BuylistController();
                    CardData = BuylistController.GetCards(SearchQuery);
                    CardData = PageCards(CardData);
                }
                else
                {
                    BuylistController BuylistController = new BuylistController();
                    CardData = BuylistController.GetCards();
                    CardData = PageCards(CardData);
                }
            }

        }//End OnPost

        public void OnPostAdd(string ID, int currentPage)
        {
            CurrentPage = currentPage == 0 ? 1 : currentPage;
            bool Confirmation;
            BuylistController BuylistController = new BuylistController();

            Confirmation = BuylistController.AddToBuylist(ID);

            if (Confirmation == true)
            {
                Message = "Successfully added the card to the buylist.";
            }
            else
            {
                Message = "Something went wrong when adding the card to the buylist. The card may already be on the buylist.";
            }

            List<Card> CardList = new List<Card>();
            //CardList = BuylistController.GetCards();

            if (SearchQuery != null && SearchQuery != "")
            {
                CardData = BuylistController.GetCards(SearchQuery);
                CardData = PageCards(CardData);
            }
            else
            {
                CardData = BuylistController.GetCards();
                CardData = PageCards(CardData);
            }
        }

        public List<Card> PageCards(List<Card> CardData)
        {
            Count = CardData.Count();

            if (CurrentPage > TotalPages)
            {
                CurrentPage = TotalPages;
            }

            foreach (var Card in CardData)
            {
                Card.Price = decimal.Round(Card.Price, 2);
            }

            CardData = CardData
                        .Skip((CurrentPage - 1) * PageSize)
                        .Take(PageSize)
                        .ToList();

            return CardData;
        }//End PageCards
    }//End Class
}

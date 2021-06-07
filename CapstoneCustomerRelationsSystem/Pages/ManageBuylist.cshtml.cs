using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using CapstoneCustomerRelationsSystem.Domain;
using CapstoneCustomerRelationsSystem.Domain.Models;

namespace CapstoneCustomerRelationsSystem.Pages
{
    public class ManageBuylistModel : PageModel
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
        public List<BuylistCard> CardData { get; set; }
        public string Message { get; set; }
        public IActionResult OnGet(int currentPage)
        {
            if (User.IsInRole("Admin"))
            {
                CurrentPage = currentPage == 0 ? 1 : currentPage;
                BuylistController BuylistController = new BuylistController();
                List<BuylistCard> Buylist = new List<BuylistCard>();

                CurrentPage = currentPage == 0 ? 1 : currentPage;

                if (SearchQuery != null && SearchQuery != "")
                {
                    CardData = BuylistController.GetBuylist(SearchQuery);
                    CardData = PageCards(CardData);
                }
                else
                {
                    CardData = BuylistController.GetBuylist();
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
            if (Button == "Search")
            {
                CurrentPage = currentPage == 0 ? 1 : currentPage;
                BuylistController BuylistController = new BuylistController();
                if (SearchQuery != null || SearchQuery != "")
                {
                    CardData = BuylistController.GetBuylist(SearchQuery);
                    CardData = PageCards(CardData);
                }
                else
                {
                    CardData = BuylistController.GetBuylist();
                    CardData = PageCards(CardData);
                }
            }

        }//End OnPost

        public void OnPostDelete(int BuylistID, int currentPage)
        {
            CurrentPage = currentPage == 0 ? 1 : currentPage;
            bool Confirmation;
            BuylistController BuylistController = new BuylistController();

            Confirmation = BuylistController.DeleteFromBuylist(BuylistID);

            if (Confirmation == true)
            {
                Message = "Successfully deleted the card to the buylist.";
            }
            else
            {
                Message = "Something went wrong when adding the card to the buylist. The card may already be deleted from the buylist.";
            }

            if (SearchQuery != null || SearchQuery != "")
            {
                CardData = BuylistController.GetBuylist(SearchQuery);
                CardData = PageCards(CardData);
            }
            else
            {
                CardData = BuylistController.GetBuylist();
                CardData = PageCards(CardData);
            }
        }//End OnPostDelete

        public List<BuylistCard> PageCards(List<BuylistCard> CardData)
        {
            if (CardData != null)
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
            }
            return CardData;
        }//End PageCards
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using CapstoneCustomerRelationsSystem.Domain;
using CapstoneCustomerRelationsSystem.Domain.Models;

namespace CapstoneCustomerRelationsSystem.Pages
{
    public class BuylistModel : PageModel
    {
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 50;
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        public bool EnablePrevious => CurrentPage > 1;
        public bool EnableNext => CurrentPage < TotalPages;
        [BindProperty]
        public List<BuylistCard> CardData { get; set; }
        [BindProperty]
        public string Button { get; set; }
        [BindProperty]
        public string SearchQuery { get; set; }
        public void OnGet(int currentPage)
        {
            CurrentPage = currentPage == 0 ? 1 : currentPage;
            BuylistController BuylistController = new BuylistController();

            List<BuylistCard> Buylist = new List<BuylistCard>();

            Buylist = BuylistController.GetBuylist();

            CardData = Buylist;
            CardData = PageCards(CardData);
        }//End OnGet

        public void OnPost(int currentPage)
        {
            if (Button == "Search")
            {
                if (SearchQuery != null || SearchQuery != "")
                {
                    BuylistController BuylistController = new BuylistController();
                    List<BuylistCard> Buylist = new List<BuylistCard>();
                    CurrentPage = currentPage == 0 ? 1 : currentPage;

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
            }
        }//End OnPost

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

                return CardData;
            }
            return CardData;
        }//End PageCards
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using CapstoneCustomerRelationsSystem.Domain;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System;
using System.Linq;

namespace CapstoneCustomerRelationsSystem.Pages
{
    public class ViewMTGCardsModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 50;
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        public bool EnablePrevious => CurrentPage > 1;
        public bool EnableNext => CurrentPage < TotalPages;
        [BindProperty]
        public string ID { get; set; }
        [BindProperty]
        public string Button { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Prompt = "Search...")]
        public string SearchQuery { get; set; }
        [BindProperty]
        public List<Card> CardData { get; set; }

        public void OnGet(int currentPage)
        {
            CurrentPage = currentPage == 0 ? 1 : currentPage;
            MTGCardController MTGCardController = new MTGCardController();
            CardData = MTGCardController.GetCards();

            if (SearchQuery != null)
            {
                PageSearch(SearchQuery);
            }

            CardData = PageCards(CardData);
        }//End OnGet

        public void OnPost()
        {
            if (Button == "Search")
            {
                PageSearch(SearchQuery);
            }
        }//End OnPost

        public void PageSearch(string SearchQuery)
        {
            if (SearchQuery != null && SearchQuery != "")
            {
                MTGCardController MTGCardController = new MTGCardController();
                CardData = MTGCardController.GetCards(SearchQuery);
                CardData = PageCards(CardData);
            }
            else
            {
                MTGCardController MTGCardController = new MTGCardController();
                CardData = MTGCardController.GetCards();
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
    }
}


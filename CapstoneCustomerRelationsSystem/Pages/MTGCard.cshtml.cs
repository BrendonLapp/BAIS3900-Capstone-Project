using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapstoneCustomerRelationsSystem.Domain;

namespace CapstoneCustomerRelationsSystem.Pages
{
    public class MTGCardModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string CardID { get; set; }
        [BindProperty]
        public Card DisplayCard { get; set; }
        [BindProperty]
        public int Quantity { get; set; }
        [BindProperty]
        public decimal Price { get; set; }
        public void OnGet()
        {
            MTGCardController MTGCardController = new MTGCardController();
            DisplayCard = MTGCardController.GetCard(CardID);
            DisplayCard.Price = decimal.Round(DisplayCard.Price, 2);
        }//End OnGet

        public IActionResult OnPost()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("Login");
            }    
            else
            {
                bool Success;
                CartController CartController = new CartController();
                MTGCardController MTGCardController = new MTGCardController();
                Card MTGCard = new Card();
                MTGCard = MTGCardController.GetCard(CardID);
                CRS Requests = new CRS();

                int UserAccountNumber = Requests.GetUserAccountNumber(User.Identity.Name);

                Success = CartController.AddCardToCart(CardID, Quantity, UserAccountNumber, MTGCard.Price);

                DisplayCard = MTGCardController.GetCard(CardID);
                return Page();
            }
        }//End OnPost
    }
}

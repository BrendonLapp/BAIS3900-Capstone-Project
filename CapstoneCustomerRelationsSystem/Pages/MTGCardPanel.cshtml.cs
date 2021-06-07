using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapstoneCustomerRelationsSystem.Domain;

namespace CapstoneCustomerRelationsSystem.Pages
{
    public class MTGCardPanelModel : PageModel
    {
        [BindProperty]
        public string Button { get; set; }
        public string Message { get; set; }
        public void OnGet()
        {
        }//End OnGet

        public void OnPost()
        {
            bool Confirmation = false;
            if (Button == "Add New Cards")
            {
                MTGCardController MTGCardController = new MTGCardController();
                Confirmation = MTGCardController.AddCardsToDatabase();

                if (Confirmation == true)
                {
                    Message = "Successfully added new cards to the database.";
                }
                else
                {
                    Message = "No new cards were added to the database";
                }
            }
            if (Button == "Update Prices")
            {
                MTGCardController MTGCardController = new MTGCardController();
                Confirmation = MTGCardController.UpdateCardPrices();

                if (Confirmation == true)
                {
                    Message = "Successfully updated the card prices.";
                }
                else
                {
                    Message = "Failed to update the card prices.";
                }
            }
        }//OnPost
    }
}

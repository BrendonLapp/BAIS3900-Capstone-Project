using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapstoneCustomerRelationsSystem.Domain;

namespace CapstoneCustomerRelationsSystem.Pages
{
    public class ProductModel : PageModel 
    {
        public Product SearchedProduct { get; set; }

        private CRS RequestDirector = new CRS();
        [BindProperty]
        public int ProductID { get; set; }
        [BindProperty]
        public int Quantity { get; set; }

        public async Task<IActionResult> OnGet() {
            IActionResult pageToReturn = Page();
            //Gets the product number from the url. ?product=1
            int productNumber;
            if (int.TryParse(Request.Query["product"], out productNumber)) {
                if ((SearchedProduct = RequestDirector.RetrieveProduct(productNumber)) != null) {

                } else {
                    pageToReturn = RedirectToPage("/Catalog");
                }
            } else {
                //If the product can't be found, redirect to the Catalog page.
                pageToReturn = RedirectToPage("/Catalog");
            }
            return pageToReturn;
        }//end OnGet

        public IActionResult OnPost() {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("Login");
            }
            else
            {
                bool Success;
                CartController CartController = new CartController();
                CRS Requests = new CRS();
                Product Product = new Product();
                Product = Requests.RetrieveProduct(ProductID);

                int UserAccountNumber = Requests.GetUserAccountNumber(User.Identity.Name);

                Success = CartController.AddProductToCart(ProductID, Quantity, UserAccountNumber, Product.ProductPrice);

                SearchedProduct = Requests.RetrieveProduct(ProductID);
                return Page();
            }
        }//end OnPost
    }//end class
}//end namespace
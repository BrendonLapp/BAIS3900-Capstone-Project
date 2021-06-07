using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapstoneCustomerRelationsSystem.Domain;

namespace CapstoneCustomerRelationsSystem.Pages
{
    [Authorize(Roles = "Admin")]
    public class ManageProductsModel : PageModel 
    {
        private CRS RequestDirector = new CRS();
        public List<Product> Products = new List<Product>();

        [BindProperty]
        public string SearchBar { get; set; }
        [BindProperty]
        public string Submit { get; set; }

        [BindProperty]
        public int ProductNumber { get; set; }
        [BindProperty]
        public string ImageURL { get; set; }
        [BindProperty]
        public string ProductLink { get; set; }
        [BindProperty]
        public string CompanyName { get; set; }
        [BindProperty]
        public string ProductName { get; set; }
        [BindProperty]
        public string ProductDescription { get; set; }
        [BindProperty]
        public decimal ProductPrice { get; set; }

        public void OnGet() {
            PopulateWithProducts(SearchBar);
        }//end OnGet

        public void OnPost() {
            switch (Submit) {
                case "Search":
                    PopulateWithProducts(SearchBar);
                    break;
                case "Add":
                    // PopulateWithProducts(SearchBar);
                    ValidateFields();
                    //Check if the form is valid
                    if (ModelState.IsValid) {           
                        RequestDirector.MakeProduct(
                            new Product {
                                ImageURL = ImageURL,
                                // ProductLink = ProductLink,
                                CompanyName = CompanyName,
                                ProductName = ProductName,
                                ProductDescription = ProductDescription,
                                ProductPrice = ProductPrice
                            });
                    }
                    PopulateWithProducts(SearchBar);
                    break;
                case "Update":
                    PopulateWithProducts(SearchBar);
                    ValidateFields();
                    if (ModelState.IsValid) {
                        if (ProductNumber != 0) {
                            RequestDirector.ModifyProduct(
                                new Product {
                                    ProductNumber = ProductNumber,
                                    ImageURL = ImageURL,
                                    // ProductLink = ProductLink,
                                    CompanyName = CompanyName,
                                    ProductName = ProductName,
                                    ProductDescription = ProductDescription,
                                    ProductPrice = ProductPrice
                                });
                        }
                    }
                    break;
                case "Delete":
                    PopulateWithProducts(SearchBar);
                    if (ProductNumber != 0) {
                        RequestDirector.RemoveProduct(ProductNumber);
                    }
                    //Clear the fields
                    ProductNumber = 0;
                    ImageURL = "";
                    ProductLink = "";
                    CompanyName = "";
                    ProductName = "";
                    ProductDescription = "";
                    ProductPrice = 0;
                    PopulateWithProducts(SearchBar);
                    break;
                default:
                    PopulateWithProducts(SearchBar);
                        if (Submit.Contains("PopulateUpdateForm")) {
                            Submit = Submit.Remove(0, 18);
                            //Retrieve Product then populate form
                            Product foundProduct = RequestDirector.RetrieveProduct(int.Parse(Submit));

                            ProductNumber = foundProduct.ProductNumber;
                            ImageURL = foundProduct.ImageURL;
                            // ProductLink = foundProduct.ProductLink;
                            CompanyName = foundProduct.CompanyName;
                            ProductName = foundProduct.ProductName;
                            ProductDescription = foundProduct.ProductDescription;
                            ProductPrice = foundProduct.ProductPrice;
                    }
                    break;
            }
            SearchBar = SearchBar;
        }//end OnPost

        private void PopulateWithProducts() {
            Products = RequestDirector.RetrieveProducts();
        }//end PopulateWithProducts

        ///<summary>Populates the Products list with products related to the search term.
        ///<para>Searches by name.</para>
        ///</summary>
        private void PopulateWithProducts(string _searchTerm) {
            if (_searchTerm != null && _searchTerm != "") {
                Products = RequestDirector.RetrieveProducts().Where(_product => 
                    _product.ProductName.ToLower()
                                                                            .Contains(_searchTerm.ToLower())).ToList();
            } else {
                PopulateWithProducts();
            }
        }//end PopulateWithProducts

        private void ValidateFields() {
            if (ImageURL == null || ImageURL == "") {
                ModelState.AddModelError("ImageURL", "*An Image URL is required.");
            } else {
                if (ImageURL.Length > 200) {
                    ModelState.AddModelError("ImageURL", "*The character limit is 200, please make the URL shorter.");
                }
            }
            // if (ProductLink == null || ProductLink == "") {
            //     ModelState.AddModelError("ProductLink", "*A Product Link is required.");
            // } else {
            //     if (ProductLink.Length > 200) {
            //         ModelState.AddModelError("ProductLink", "*The character limit is 200, please make the URL shorter.");
            //     }
            // }
            if (CompanyName == null || CompanyName == "") {
                ModelState.AddModelError("CompanyName", "*The Company Name is required.");
            } else {
                if (CompanyName.Length > 200) {
                    ModelState.AddModelError("CompanyName", "*The character limit is 50, please make the Company Name shorter.");
                }
            }
            if (ProductName == null || ProductName == "") {
                ModelState.AddModelError("ProductName", "*The Product Name is required.");
            } else {
                if (ProductName.Length > 200) {
                    ModelState.AddModelError("ProductName", "*The character limit is 1000, please make the Product Name shorter.");
                }
            }
            if (ProductDescription == null || ProductDescription == "") {
                ModelState.AddModelError("ProductDescription", "*The Product Description is required.");
            } else {
                if (ProductDescription.Length > 200) {
                    ModelState.AddModelError("ProductDescription", "*The character limit is 2000, please make the Product Description shorter.");
                }
            }
            if (ProductPrice <= 0) {
                ModelState.AddModelError("ProductPrice", "*The Product Price is must be greater than 0.");
            }
        }//end ValidateFields
        
    }//end class
}//end namespace
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CapstoneCustomerRelationsSystem.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace CapstoneCustomerRelationsSystem.Pages 
{
    public class CatalogModel : PageModel 
    {
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 50;
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        public bool EnablePrevious => CurrentPage > 1;
        public bool EnableNext => CurrentPage < TotalPages;
        private CRS RequestDirector = new CRS();
        public List<Product> Products = new List<Product>();

        [BindProperty]
        public string SearchBar { get; set; }
        [BindProperty]
        public string Submit { get; set; }

        public void OnGet(int currentPage) {
            CurrentPage = currentPage == 0 ? 1 : currentPage;
            PopulateWithProducts();
        }//end OnGet

        public void OnPost() {
            switch (Submit) {
                case "Search":
                    PopulateWithProducts(SearchBar);
                    break;
                default:
                    PopulateWithProducts();
                    break;
            }
        }//end OnPost

        private void PopulateWithProducts() {
            Products = RequestDirector.RetrieveProducts();
            Products = PageProducts(Products);
        }//end PopulateWithProducts

        ///<summary>Populates the Products list with products related to the search term.
        ///<para>Searches by name.</para>
        ///</summary>
        private void PopulateWithProducts(string _searchTerm) {
            if (_searchTerm != null) {
                Products = RequestDirector.RetrieveProducts().Where(_product => 
                    _product.ProductName.ToLower()
                                                                            .Contains(_searchTerm.ToLower())).ToList();
                Products = PageProducts(Products);
            } else {
                PopulateWithProducts();
            }
        }//end PopulateWithProducts

        public List<Product> PageProducts(List<Product> Products)
        {
            Count = Products.Count();

            if (CurrentPage > TotalPages)
            {
                CurrentPage = TotalPages;
            }

            Products = Products
                        .Skip((CurrentPage - 1) * PageSize)
                        .Take(PageSize)
                        .ToList();

            return Products;
        }//End PageProducts

    }//end class
}//end namespace
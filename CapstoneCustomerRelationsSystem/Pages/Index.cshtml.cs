using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapstoneCustomerRelationsSystem.Domain;
using CapstoneCustomerRelationsSystem.TechnicalServices;

namespace CapstoneCustomerRelationsSystem.Pages
{
    public class IndexModel : PageModel 
    {
        private CRS RequestDirector = new CRS();
        public List<NewsItem> NewsItems { get; set; } = new List<NewsItem>();


        public void OnGet() {
            PopulateWithNews();
        }//end OnGet

        public void OnPost() {

        }//end OnPost

        private void PopulateWithNews () {
            NewsItems = RequestDirector.RetrieveNewsItems();
        }
    }//end class
}//end namespace
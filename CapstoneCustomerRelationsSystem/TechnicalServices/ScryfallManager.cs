using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using CapstoneCustomerRelationsSystem.Domain.Models.ScryFall;

namespace CapstoneCustomerRelationsSystem.TechnicalServices
{
    public class ScryfallManager
    {
        public async Task<ScryFallRoot> GetCardsFromScryFall()
        {
            ScryFallRoot ScryFall = new ScryFallRoot();

            using (var HttpClient = new HttpClient())
            {
                using (var Response = await HttpClient.GetAsync("https://api.scryfall.com/cards/search?q=layout%3Anormal"))
                {
                    string ApiResponse = await Response.Content.ReadAsStringAsync();
                    ScryFall = JsonConvert.DeserializeObject<ScryFallRoot>(ApiResponse);
                }
            }

            return ScryFall;
        }//GetCardsFromScryFall

        public async Task<ScryFallRoot> GetCardsFromScryFall(string PageUrl)
        {
            ScryFallRoot ScryFall = new ScryFallRoot();

            using (var HttpClient = new HttpClient())
            {
                using (var Response = await HttpClient.GetAsync(PageUrl))
                {
                    string ApiResponse = await Response.Content.ReadAsStringAsync();
                    ScryFall = JsonConvert.DeserializeObject<ScryFallRoot>(ApiResponse);
                }
            }

            return ScryFall;
        }//End GetCardsFromScryFall

        public async Task<ScryFallData> GetCardsFromScryFallByID(string ID)
        {
            string URL = "https://api.scryfall.com/cards/" + ID;

            ScryFallData ScryFall = new ScryFallData();

            using (var HttpClient = new HttpClient())
            {
                using (var Response = await HttpClient.GetAsync(URL))
                {
                    string ApiReponse = await Response.Content.ReadAsStringAsync();
                    ScryFall = JsonConvert.DeserializeObject<ScryFallData>(ApiReponse);
                }
            }

            return ScryFall;
        }//End GetCardsFromScryFallByID

        public async Task<SetsRoot> GetAllSets()
        {
            SetsRoot SetRoot = new SetsRoot();

            using (var HttpClient = new HttpClient())
            {
                using (var Response = await HttpClient.GetAsync("https://api.scryfall.com/sets"))
                {
                    string ApiResponse = await Response.Content.ReadAsStringAsync();
                    SetRoot = JsonConvert.DeserializeObject<SetsRoot>(ApiResponse);
                }
            }

            return SetRoot;
        }//End GetAllSets

        public async Task<ScryFallRoot> GetCardsInSet(string SetCode)
        {
            ScryFallRoot ScryFallRoot = new ScryFallRoot();
            string Url = "https://api.scryfall.com/cards/search?order=set&q=e%3A" + SetCode + "&unique=prints";

            using (var HttpClient = new HttpClient())
            {
                using (var Response = await HttpClient.GetAsync(Url))
                {
                    string ApiResponse = await Response.Content.ReadAsStringAsync();
                    ScryFallRoot = JsonConvert.DeserializeObject<ScryFallRoot>(ApiResponse);
                }
            }
            return ScryFallRoot;
        }//End GetCardsInSet
    }
}

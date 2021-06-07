using System.Collections.Generic;
using CapstoneCustomerRelationsSystem.Domain.Models;
using CapstoneCustomerRelationsSystem.TechnicalServices;

namespace CapstoneCustomerRelationsSystem.Domain
{
    public class CartController
    {
        public bool AddCardToCart(string CardID, int Quantity, int UserAccountNumber, decimal Price)
        {
            List<Cart> ExistingCart = new List<Cart>();
            ExistingCart = GetCart(UserAccountNumber);
            bool Success;

            CartManager CartManager = new CartManager();
            
            try
            {
                Success = CartManager.AddCardToCart(CardID, Quantity, UserAccountNumber, Price);
            }
            catch
            {
                Success = false;
            }

            return Success;
        }//End AddCardtoCart

        public bool AddProductToCart(int ProductID, int Quantity, int UserAccountNumber, decimal Price)
        {
            bool Success;
            CartManager CartManager = new CartManager();

            try
            {
                Success = CartManager.AddProductToCart(ProductID, Quantity, UserAccountNumber, Price);
            }
            catch
            {
                Success = false;
            }

            return Success;
        }//End AddProductToCart

        public bool DeleteFromCart(int CartID)
        {
            bool Success;
            CartManager CartManager = new CartManager();

            try
            {
                Success = CartManager.DeleteFromcart(CartID);
            }
            catch
            {
                Success = false;
            }

            return Success;
        }//End DeleteFromCart

        public List<Cart> GetCart(int UserAccountNumber)
        {
            CRS Request = new CRS();
            MTGCardController MTGCardController = new MTGCardController();
            List<Cart> Cart = new List<Cart>();
            CartManager CartManager = new CartManager();
            try
            {
                Cart = CartManager.GetCart(UserAccountNumber);

                foreach (var Item in Cart)
                {
                    int ProductID;
                    bool IsProduct = int.TryParse(Item.ID, out ProductID);
                    if (IsProduct == true)
                    {
                        Product FoundProduct = new Product();
                        FoundProduct = Request.RetrieveProduct(int.Parse(Item.ID));
                        Item.ImageLink = FoundProduct.ImageURL;
                        Item.Name = FoundProduct.ProductName;
                    }
                    else
                    {
                        Card FoundCard = new Card();
                        FoundCard = MTGCardController.GetCard(Item.ID);
                        Item.ImageLink = FoundCard.ImageSmall;
                        Item.Name = FoundCard.Name;
                    }
                    Item.Price = decimal.Round(Item.Price, 2);
                }
            }
            catch
           {
                Cart = null;
            }

            return Cart;
        }//End GetCart

        public bool DeleteCart(int UserAccountNumber)
        {
            bool Success;
            CartManager CartManager = new CartManager();

            try
            {
                Success = CartManager.DeleteCart(UserAccountNumber);
            }
            catch
            {
                Success = false;
            }

            return Success;
        }//End DeleteCart

        public bool UpdateCart(List<Cart> Cart)
        {
            bool Success = true;
            CartManager cartManager = new CartManager();

            try
            {
                while (Success == true)
                {
                    foreach (var Item in Cart)
                    {
                        Success = cartManager.UpdateCart(Item);
                    }
                }
            }
            catch
            {
                Success = false;
            }

            return Success;
        }
    }
}

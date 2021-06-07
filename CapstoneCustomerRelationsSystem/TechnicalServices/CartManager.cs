using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CapstoneCustomerRelationsSystem.Domain.Models;

namespace CapstoneCustomerRelationsSystem.TechnicalServices
{
    public class CartManager
    {
        private SqlConnection Connection { get; set; } = new SqlConnection
        {
            ConnectionString = Startup.CRSConnectionString
        };

        public bool AddCardToCart(string CardID, int Quantity, int UserAccountNumber, decimal Price)
        {
            bool Success;

            SqlConnection CapstoneDB = new SqlConnection();
            CapstoneDB.ConnectionString = Connection.ConnectionString;
            CapstoneDB.Open();

            SqlCommand AddToCart = new SqlCommand
            {
                CommandText = "AddCardToCart",
                CommandType = CommandType.StoredProcedure,
                Connection = CapstoneDB
            };

            SqlParameter CardIDParameter = new SqlParameter
            {
                ParameterName = "CardID",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.VarChar,
                SqlValue = CardID
            };
            AddToCart.Parameters.Add(CardIDParameter);

            SqlParameter QuantityParameter = new SqlParameter
            {
                ParameterName = "Quantity",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
                SqlValue = Quantity
            };
            AddToCart.Parameters.Add(QuantityParameter);

            SqlParameter UserAccountNumberParameter = new SqlParameter
            {
                ParameterName = "UserAccountNumber",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
                SqlValue = UserAccountNumber
            };
            AddToCart.Parameters.Add(UserAccountNumberParameter);

            AddToCart.ExecuteNonQuery();

            CapstoneDB.Close();
            Success = true;
            return Success;
        }//End AddCardToCart

        public bool AddProductToCart(int ProductID, int Quantity, int UserAccountNumber, decimal Price)
        {
            bool Success;

            SqlConnection CapstoneDB = new SqlConnection();
            CapstoneDB.ConnectionString = Connection.ConnectionString;
            CapstoneDB.Open();

            SqlCommand AddToCart = new SqlCommand
            {
                CommandText = "AddProductToCart",
                CommandType = CommandType.StoredProcedure,
                Connection = CapstoneDB
            };

            SqlParameter ProductIDParameter = new SqlParameter
            {
                ParameterName = "ProductID",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
                SqlValue = ProductID
            };
            AddToCart.Parameters.Add(ProductIDParameter);

            SqlParameter QuantityParameter = new SqlParameter
            {
                ParameterName = "Quantity",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
                SqlValue = Quantity
            };
            AddToCart.Parameters.Add(QuantityParameter);

            SqlParameter UserAccountNumberParameter = new SqlParameter
            {
                ParameterName = "UserAccountNumber",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
                SqlValue = UserAccountNumber
            };
            AddToCart.Parameters.Add(UserAccountNumberParameter);

            AddToCart.ExecuteNonQuery();

            CapstoneDB.Close();
            Success = true;
            return Success;
        }//End AddProductToCart

        internal bool UpdateCart(Cart item)
        {
            bool Success;

            SqlConnection CapstoneDB = new SqlConnection();
            CapstoneDB.ConnectionString = Connection.ConnectionString;
            CapstoneDB.Open();

            SqlCommand UpdateTheCart = new SqlCommand
            {
                CommandText = "UpdateCart",
                CommandType = CommandType.StoredProcedure,
                Connection = CapstoneDB
            };

            SqlParameter CartIDParameter = new SqlParameter
            {
                ParameterName = "CartID",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
                SqlValue = item.CartID
            };
            UpdateTheCart.Parameters.Add(CartIDParameter);

            SqlParameter QuantityParameter = new SqlParameter
            {
                ParameterName = "Quantity",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
                SqlValue = item.Quantity
            };
            UpdateTheCart.Parameters.Add(QuantityParameter);

            UpdateTheCart.ExecuteNonQuery();

            CapstoneDB.Close();
            Success = true;
            return Success;
        }//End UpdateCart

        public bool DeleteFromcart(int CartID)
        {
            bool Success;

            SqlConnection CapstoneDB = new SqlConnection();
            CapstoneDB.ConnectionString = Connection.ConnectionString;
            CapstoneDB.Open();

            SqlCommand DeleteFromCart = new SqlCommand
            {
                CommandText = "DeleteFromCart",
                CommandType = CommandType.StoredProcedure,
                Connection = CapstoneDB
            };

            SqlParameter CartIDParameter = new SqlParameter
            {
                ParameterName = "CartID",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
                SqlValue = CartID
            };
            DeleteFromCart.Parameters.Add(CartIDParameter);

            DeleteFromCart.ExecuteNonQuery();

            CapstoneDB.Close();
            Success = true;
            return Success;
        }//End DeleteFromCart

        public bool DeleteCart(int UserAccountNumber)
        {
            bool Confirmation;

            SqlConnection CapstoneDB = new SqlConnection();
            CapstoneDB.ConnectionString = Connection.ConnectionString;
            CapstoneDB.Open();

            SqlCommand DeleteCart = new SqlCommand
            {
                CommandText = "DeleteCart",
                CommandType = CommandType.StoredProcedure,
                Connection = CapstoneDB
            };

            SqlParameter UserAccountNumberParameter = new SqlParameter
            {
                ParameterName = "UserAccountNumber",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
                SqlValue = UserAccountNumber
            };
            DeleteCart.Parameters.Add(UserAccountNumberParameter);

            DeleteCart.ExecuteNonQuery();

            CapstoneDB.Close();

            Confirmation = true;

            return Confirmation;
        }//End DeleteCart

        public List<Cart> GetCart(int UserAccountNumber)
        {
            List<Cart> Cart = new List<Cart>();

            SqlConnection CapstoneDB = new SqlConnection();
            CapstoneDB.ConnectionString = Connection.ConnectionString;
            CapstoneDB.Open();

            SqlCommand GetCart = new SqlCommand
            {
                CommandText = "GetCart",
                CommandType = CommandType.StoredProcedure,
                Connection = CapstoneDB
            };

            SqlParameter UserAccountNumberParameter = new SqlParameter
            {
                ParameterName = "UserAccountNumber",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
                SqlValue = UserAccountNumber
            };
            GetCart.Parameters.Add(UserAccountNumberParameter);

            SqlDataReader DataReader = GetCart.ExecuteReader();

            if (DataReader.HasRows)
            {
                while (DataReader.Read())
                {
                    Cart CartItem = new Cart();
                    CartItem.CartID = int.Parse(DataReader["CartID"].ToString());
                    if (DataReader["CardID"].ToString() == "" || DataReader["CardID"].ToString() == null)
                    {
                        CartItem.ID = DataReader["ProductNumber"].ToString();
                    }
                    else if (DataReader["ProductNumber"].ToString() == "" || DataReader["ProductNumber"].ToString() == null)
                    {
                        CartItem.ID = DataReader["CardID"].ToString();
                    }
                    CartItem.Quantity = int.Parse(DataReader["Quantity"].ToString());
                    CartItem.UserAccountNumber = int.Parse(DataReader["UserAccountNumber"].ToString());
                    CartItem.Price = decimal.Parse(DataReader["Price"].ToString());
                    Cart.Add(CartItem);
                }
            }

            CapstoneDB.Close();

            return Cart;
        }//End GetCart
    }
}

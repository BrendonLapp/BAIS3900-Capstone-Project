using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CapstoneCustomerRelationsSystem.Domain.Models;

namespace CapstoneCustomerRelationsSystem.TechnicalServices
{
    public class BuylistManager
    {
        private SqlConnection Connection { get; set; } = new SqlConnection {
            ConnectionString = Startup.CRSConnectionString
        };

        public bool AddToBuylist(string CardID)
        {
            bool Success;

            SqlConnection CapstoneDB = new SqlConnection();
            CapstoneDB.ConnectionString = Connection.ConnectionString;
            CapstoneDB.Open();

            SqlCommand AddToBuylist = new SqlCommand
            {
                CommandText = "AddToBuylist",
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
            AddToBuylist.Parameters.Add(CardIDParameter);

            AddToBuylist.ExecuteNonQuery();
            CapstoneDB.Close();

            Success = true;
            return Success;
        }//End AddToBuylist

        public List<BuylistCard> GetBuylist()
        {
            List<BuylistCard> Buylist = new List<BuylistCard>();

            SqlConnection CapstoneDB = new SqlConnection();
            CapstoneDB.ConnectionString = Connection.ConnectionString;
            CapstoneDB.Open();

            SqlCommand GetBuylistCommand = new SqlCommand
            {
                CommandText = "GetBuylist",
                CommandType = CommandType.StoredProcedure,
                Connection = CapstoneDB
            };

            SqlDataReader DataReader = GetBuylistCommand.ExecuteReader();

            if (DataReader.HasRows)
            {
                while (DataReader.Read())
                {
                    BuylistCard Card = new BuylistCard();

                    for (int index = 0; index < DataReader.FieldCount; index++)
                    {
                        Card.BuylistID = DataReader["BuylistID"].ToString();
                        Card.Id = DataReader["CardID"].ToString();
                        Card.Name = DataReader["CardName"].ToString();
                        Card.Uri = DataReader["URI"].ToString();
                        Card.Set = DataReader["Set"].ToString();
                        Card.SetName = DataReader["SetName"].ToString();
                        Card.Collector_Number = DataReader["CollectorNumber"].ToString();
                        Card.Rarity = DataReader["Rarity"].ToString();
                        Card.ImageSmall = DataReader["ImageSmall"].ToString();
                        Card.ImageNormal = DataReader["ImageNormal"].ToString();
                        Card.ImageLarge = DataReader["ImageLarge"].ToString();
                        Card.Price = decimal.Parse(DataReader["Price"].ToString());
                    }

                    Buylist.Add(Card);
                }
            }

            DataReader.Close();
            CapstoneDB.Close();

            return Buylist;
        }//end GetBuylist

        public List<BuylistCard> GetBuylist(string Query)
        {
            List<BuylistCard> Buylist = new List<BuylistCard>();

            SqlConnection CapstoneDB = new SqlConnection();
            CapstoneDB.ConnectionString = Connection.ConnectionString;
            CapstoneDB.Open();

            SqlCommand GetBuylistByQuery = new SqlCommand
            {
                CommandText = "GetBuylistByQuery",
                CommandType = CommandType.StoredProcedure,
                Connection = CapstoneDB
            };

            SqlParameter QueryParameter = new SqlParameter
            {
                ParameterName = "Query",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.VarChar,
                SqlValue = Query
            };
            GetBuylistByQuery.Parameters.Add(QueryParameter);

            SqlDataReader DataReader = GetBuylistByQuery.ExecuteReader();

            if (DataReader.HasRows)
            {
                while (DataReader.Read())
                {
                    BuylistCard Card = new BuylistCard();

                    for (int index = 0; index < DataReader.FieldCount; index++)
                    {
                        Card.BuylistID = DataReader["BuylistID"].ToString();
                        Card.Id = DataReader["CardID"].ToString();
                        Card.Name = DataReader["CardName"].ToString();
                        Card.Uri = DataReader["URI"].ToString();
                        Card.Set = DataReader["Set"].ToString();
                        Card.SetName = DataReader["SetName"].ToString();
                        Card.Collector_Number = DataReader["CollectorNumber"].ToString();
                        Card.Rarity = DataReader["Rarity"].ToString();
                        Card.ImageSmall = DataReader["ImageSmall"].ToString();
                        Card.ImageNormal = DataReader["ImageNormal"].ToString();
                        Card.ImageLarge = DataReader["ImageLarge"].ToString();
                        Card.Price = decimal.Parse(DataReader["Price"].ToString());
                    }

                    Buylist.Add(Card);
                }
            }

            DataReader.Close();
            CapstoneDB.Close();

            return Buylist;
        }//End GetBuylist

        public bool DeleteFromBuylist(int BuylistID)
        {
            bool Success;

            SqlConnection CapstoneDB = new SqlConnection();
            CapstoneDB.ConnectionString = Connection.ConnectionString;
            CapstoneDB.Open();

            SqlCommand DeleteFromBuylistCommand = new SqlCommand
            {
                CommandText = "DeleteFromBuylist",
                CommandType = CommandType.StoredProcedure,
                Connection = CapstoneDB
            };

            SqlParameter BuylistIDParameter = new SqlParameter
            {
                ParameterName = "BuylistID",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.VarChar,
                SqlValue = BuylistID
            };
            DeleteFromBuylistCommand.Parameters.Add(BuylistIDParameter);

            DeleteFromBuylistCommand.ExecuteNonQuery();
            CapstoneDB.Close();

            Success = true;
            return Success;
        }//End DeleteFromBuylist
    }
}

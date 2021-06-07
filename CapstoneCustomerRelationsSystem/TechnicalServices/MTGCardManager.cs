using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CapstoneCustomerRelationsSystem.Domain;

namespace CapstoneCustomerRelationsSystem.TechnicalServices
{
    public class MTGCardManager
    {
        private SqlConnection Connection { get; set; } = new SqlConnection {
            ConnectionString = Startup.CRSConnectionString
        };

        public bool AddMTGCard(Card NewCard)
        {
            bool Success;

            SqlConnection CapstoneDB = new SqlConnection();
            CapstoneDB.ConnectionString = Connection.ConnectionString;
            CapstoneDB.Open();

            SqlCommand AddCardToMTGCards = new SqlCommand
            {
                CommandText = "AddCardToMTGCards",
                CommandType = CommandType.StoredProcedure,
                Connection = CapstoneDB
            };

            SqlParameter CardIDParameter = new SqlParameter
            {
                ParameterName = "CardID",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.VarChar,
                SqlValue = NewCard.Id
            };
            AddCardToMTGCards.Parameters.Add(CardIDParameter);

            SqlParameter CardNameParameter = new SqlParameter
            {
                ParameterName = "CardName",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.VarChar,
                SqlValue = NewCard.Name
            };
            AddCardToMTGCards.Parameters.Add(CardNameParameter);

            SqlParameter URIParameter = new SqlParameter
            {
                ParameterName = "URI",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.VarChar,
                SqlValue = NewCard.Uri
            };
            AddCardToMTGCards.Parameters.Add(URIParameter);

            SqlParameter SetParameter = new SqlParameter
            {
                ParameterName = "Set",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.VarChar,
                SqlValue = NewCard.Set
            };
            AddCardToMTGCards.Parameters.Add(SetParameter);

            SqlParameter SetNameParameter = new SqlParameter
            {
                ParameterName = "SetName",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.VarChar,
                SqlValue = NewCard.SetName
            };
            AddCardToMTGCards.Parameters.Add(SetNameParameter);

            SqlParameter CollectorNumberParameter = new SqlParameter
            {
                ParameterName = "CollectorNumber",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.VarChar,
                SqlValue = NewCard.Collector_Number
            };
            AddCardToMTGCards.Parameters.Add(CollectorNumberParameter);

            SqlParameter RarityParameter = new SqlParameter
            {
                ParameterName = "Rarity",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.VarChar,
                SqlValue = NewCard.Rarity
            };
            AddCardToMTGCards.Parameters.Add(RarityParameter);

            SqlParameter ImageSmallParamater = new SqlParameter
            {
                ParameterName = "ImageSmall",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.VarChar,
                SqlValue = NewCard.ImageSmall
            };
            AddCardToMTGCards.Parameters.Add(ImageSmallParamater);

            SqlParameter ImageNormalParameter = new SqlParameter
            {
                ParameterName = "ImageNormal",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.VarChar,
                SqlValue = NewCard.ImageNormal
            };
            AddCardToMTGCards.Parameters.Add(ImageNormalParameter);

            SqlParameter ImageLargeParameter = new SqlParameter
            {
                ParameterName = "ImageLarge",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.VarChar,
                SqlValue = NewCard.ImageLarge
            };
            AddCardToMTGCards.Parameters.Add(ImageLargeParameter);

            SqlParameter PriceParameter = new SqlParameter
            {
                ParameterName = "Price",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Money,
                SqlValue = decimal.Round(NewCard.Price, 2)
            };
            AddCardToMTGCards.Parameters.Add(PriceParameter);

            SqlParameter LastUpdatedParameter = new SqlParameter
            {
                ParameterName = "LastUpdated",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.DateTime,
                SqlValue = DateTime.Now
            };
            AddCardToMTGCards.Parameters.Add(LastUpdatedParameter);

            AddCardToMTGCards.ExecuteNonQuery();

            CapstoneDB.Close();
            Success = true;

            return Success;
        }//End AddMTGCard

        public bool UpdateCardPrice(string CardID, decimal Price)
        {
            bool Success;

            SqlConnection CapstoneDB = new SqlConnection();
            CapstoneDB.ConnectionString = Connection.ConnectionString;
            CapstoneDB.Open();

            SqlCommand UpdateMTGCard = new SqlCommand
            {
                CommandText = "UpdateMTGCard",
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
            UpdateMTGCard.Parameters.Add(CardIDParameter);

            SqlParameter PriceParameter = new SqlParameter
            {
                ParameterName = "Price",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.VarChar,
                SqlValue = decimal.Round(Price, 2)
            };
            UpdateMTGCard.Parameters.Add(PriceParameter);

            UpdateMTGCard.ExecuteNonQuery();

            CapstoneDB.Close();

            Success = true;

            return Success;
        }//ENd UpdateCardPrices

        public List<Card> GetAllCards()
        {
            List<Card> CardList = new List<Card>();

            SqlConnection CapstoneDB = new SqlConnection();
            CapstoneDB.ConnectionString = Connection.ConnectionString;
            CapstoneDB.Open();

            SqlCommand GetMTGCards = new SqlCommand
            {
                CommandText = "GetMTGCards",
                CommandType = CommandType.StoredProcedure,
                Connection = CapstoneDB
            };

            SqlDataReader DataReader = GetMTGCards.ExecuteReader();

            if (DataReader.HasRows)
            {
                while (DataReader.Read())
                {
                    Card FoundCard = new Card
                    {
                        Id = DataReader["CardID"].ToString(),
                        Name = DataReader["CardName"].ToString(),
                        Uri = DataReader["URI"].ToString(),
                        Set = DataReader["Set"].ToString(),
                        SetName = DataReader["SetName"].ToString(),
                        Collector_Number = DataReader["CollectorNumber"].ToString(),
                        Rarity = DataReader["Rarity"].ToString(),
                        ImageSmall = DataReader["ImageSmall"].ToString(),
                        ImageNormal = DataReader["ImageNormal"].ToString(),
                        ImageLarge = DataReader["ImageLarge"].ToString(),
                        Price = decimal.Parse(DataReader["Price"].ToString())
                    };
                    CardList.Add(FoundCard);
                }
            }

            DataReader.Close();
            CapstoneDB.Close();

            return CardList;
        }//End GetAllCards

        public List<Card> GetAllCardsByQuery(string Query)
        {
            List<Card> CardList = new List<Card>();

            SqlConnection CapstoneDB = new SqlConnection();
            CapstoneDB.ConnectionString = Connection.ConnectionString;
            CapstoneDB.Open();

            SqlCommand GetAllCardsByQuery = new SqlCommand
            {
                CommandText = "GetAllCardsByQuery",
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
            GetAllCardsByQuery.Parameters.Add(QueryParameter);

            SqlDataReader DataReader = GetAllCardsByQuery.ExecuteReader();

            if (DataReader.HasRows)
            {
                while (DataReader.Read())
                {
                    Card FoundCard = new Card
                    {
                        Id = DataReader["CardID"].ToString(),
                        Name = DataReader["CardName"].ToString(),
                        Uri = DataReader["URI"].ToString(),
                        Set = DataReader["Set"].ToString(),
                        SetName = DataReader["SetName"].ToString(),
                        Collector_Number = DataReader["CollectorNumber"].ToString(),
                        Rarity = DataReader["Rarity"].ToString(),
                        ImageSmall = DataReader["ImageSmall"].ToString(),
                        ImageNormal = DataReader["ImageNormal"].ToString(),
                        ImageLarge = DataReader["ImageLarge"].ToString(),
                        Price = decimal.Parse(DataReader["Price"].ToString())
                    };
                    CardList.Add(FoundCard);
                }
            }

            DataReader.Close();
            CapstoneDB.Close();

            return CardList;
        }//End GetAllCardsByQuery

        public Card GetCard(string CardID)
        {
            Card FoundCard = new Card();

            SqlConnection CapstoneDB = new SqlConnection();
            CapstoneDB.ConnectionString = Connection.ConnectionString;
            CapstoneDB.Open();

            SqlCommand GetCardCommand = new SqlCommand
            {
                CommandText = "GetCard",
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
            GetCardCommand.Parameters.Add(CardIDParameter);

            SqlDataReader DataReader = GetCardCommand.ExecuteReader();

            if (DataReader.HasRows)
            {
                while (DataReader.Read())
                {
                    for (int index = 0; index < DataReader.FieldCount; index++)
                    {
                        FoundCard.Id = DataReader["CardID"].ToString();
                        FoundCard.Name = DataReader["CardName"].ToString();
                        FoundCard.Uri = DataReader["URI"].ToString();
                        FoundCard.Set = DataReader["Set"].ToString();
                        FoundCard.SetName = DataReader["SetName"].ToString();
                        FoundCard.Collector_Number = DataReader["CollectorNumber"].ToString();
                        FoundCard.Rarity = DataReader["Rarity"].ToString();
                        FoundCard.ImageSmall = DataReader["ImageSmall"].ToString();
                        FoundCard.ImageNormal = DataReader["ImageNormal"].ToString();
                        FoundCard.ImageLarge = DataReader["ImageLarge"].ToString();
                        FoundCard.Price = decimal.Parse(DataReader["Price"].ToString());
                    }
                }
            }

            DataReader.Close();
            CapstoneDB.Close();

            return FoundCard;
        }//End GetCard

        public List<string> GetExistingSets()
        {
            List<string> ExistingSets = new List<string>();

            SqlConnection CapstoneDB = new SqlConnection();
            CapstoneDB.ConnectionString = Connection.ConnectionString;
            CapstoneDB.Open();

            SqlCommand GetExistingSets = new SqlCommand
            {
                CommandText = "GetExistingSets",
                CommandType = CommandType.StoredProcedure,
                Connection = CapstoneDB
            };

            SqlDataReader DataReader = GetExistingSets.ExecuteReader();

            if (DataReader.HasRows)
            {
                while (DataReader.Read())
                {
                    ExistingSets.Add(DataReader["Set"].ToString());
                }
            }

            DataReader.Close();
            CapstoneDB.Close();

            return ExistingSets;
        }//End GetExistingSets
    }
}

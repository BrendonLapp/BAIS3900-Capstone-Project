using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CapstoneCustomerRelationsSystem.Domain;

namespace CapstoneCustomerRelationsSystem.TechnicalServices 
{
    public class NewsItems 
    {
        private SqlConnection Connection { get; set; } = new SqlConnection {
            ConnectionString = Startup.CRSConnectionString
        };

        public NewsItems() {

        }//end Constructor


        public List<NewsItem> GetNewsItems() {
            List<NewsItem> newsItemsToReturn = new List<NewsItem>();
            Connection.Open();
            SqlCommand command = new SqlCommand {
                Connection = Connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "GetNewsItems"
            };

            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.HasRows) {
                while (dataReader.Read()) {
                    newsItemsToReturn.Add(new NewsItem {
                        NewsItemNumber = (int)dataReader["NewsItemNumber"],
                        ImageType = (string)dataReader["ImageType"],
                        ImageURL = (string)dataReader["ImageURL"],
                        NewsItemLink = (string)dataReader["NewsItemLink"],
                        // IndexPosition = (int)dataReader["IndexPosition"],
                        OptionalNewsName = dataReader["OptionalNewsName"].GetType() == typeof(DBNull)? "": (string)dataReader["OptionalNewsName"],
                        OptionalNewsDescription = dataReader["OptionalNewsDescription"].GetType() == typeof(DBNull)? "": (string)dataReader["OptionalNewsDescription"],
                        OptionalNewsPrice = dataReader["OptionalNewsPrice"].GetType() == typeof(DBNull)? "": (string)dataReader["OptionalNewsPrice"]
                    });
                }
            }

            dataReader.Close();
            Connection.Close();
            return newsItemsToReturn;
        }//end GetNewsItems

        internal bool UpdateNewsItem(NewsItem _newsItem)
        {
            bool success = false;
            Connection.Open();
            SqlCommand command = new SqlCommand {
                CommandText = "UpdateNewsItem",
                CommandType = CommandType.StoredProcedure,
                Connection = Connection
            };

            //Create a parameter
            SqlParameter parameter = new SqlParameter {
                ParameterName = "@ImageType",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _newsItem.ImageType
            };
            //Add the parameter to the command's parameter list
            command.Parameters.Add(parameter);

            //Create a parameter
            parameter = new SqlParameter {
                ParameterName = "@NewsItemNumber",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                SqlValue = _newsItem.NewsItemNumber
            };
            //Add the parameter to the command's parameter list
            command.Parameters.Add(parameter);

            //Create a parameter
            parameter = new SqlParameter {
                ParameterName = "@ImageURL",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _newsItem.ImageURL
            };
            //Add the parameter to the command's parameter list
            command.Parameters.Add(parameter);

            //Create a parameter
            parameter = new SqlParameter {
                ParameterName = "@NewsItemLink",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _newsItem.NewsItemLink
            };
            //Add the parameter to the command's parameter list
            command.Parameters.Add(parameter);

            // //Create a parameter
            // parameter = new SqlParameter {
            //     ParameterName = "@IndexPosition",
            //     SqlDbType = SqlDbType.Int,
            //     Direction = ParameterDirection.Input,
            //     SqlValue = _newsItem.IndexPosition
            // };
            // //Add the parameter to the command's parameter list
            // command.Parameters.Add(parameter);

            //Create a parameter
            parameter = new SqlParameter {
                ParameterName = "@OptionalNewsName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _newsItem.OptionalNewsName
            };
            //Add the parameter to the command's parameter list
            command.Parameters.Add(parameter);

            //Create a parameter
            parameter = new SqlParameter {
                ParameterName = "@OptionalNewsDescription",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _newsItem.OptionalNewsDescription
            };
            //Add the parameter to the command's parameter list
            command.Parameters.Add(parameter);

            //Create a parameter
            parameter = new SqlParameter {
                ParameterName = "@OptionalNewsPrice",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _newsItem.OptionalNewsPrice
            };
            //Add the parameter to the command's parameter list
            command.Parameters.Add(parameter);
            
            // Execute the stored procedure
            command.ExecuteNonQuery();

            Connection.Close();
            success = true;
            return success;
        }//end UpdateNewsItem

        internal bool DeleteNewsItem(int _newsItemNumber) {
            bool success = false;
            Connection.Open();
            
            SqlCommand command = new SqlCommand {
                Connection = Connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "DeleteNewsItem"
            };

            SqlParameter parameter = new SqlParameter {
                ParameterName = "@NewsItemNumber",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                SqlValue = _newsItemNumber
            };
            command.Parameters.Add(parameter);

            command.ExecuteNonQuery();
            Connection.Close();
            success = true;
            return success;
        }//end DeleteNewsItem

        internal NewsItem GetNewsItem(int _newsItemNumber)
        {
            NewsItem foundNewsItem = null;
            Connection.Open();
            SqlCommand command = new SqlCommand {
                Connection = Connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "GetNewsItem"
            };

            SqlParameter parameter = new SqlParameter {
                ParameterName = "@NewsItemNumber",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                SqlValue = _newsItemNumber
            };
            command.Parameters.Add(parameter);

            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.HasRows) {
                dataReader.Read();
                foundNewsItem = new NewsItem() {
                    NewsItemNumber = (int)dataReader["NewsItemNumber"],
                    ImageType = (string)dataReader["ImageType"],
                    ImageURL = (string)dataReader["ImageURL"],
                    NewsItemLink = (string)dataReader["NewsItemLink"],
                    // IndexPosition = (int)dataReader["IndexPosition"],
                    OptionalNewsName = dataReader["OptionalNewsName"].GetType() == typeof(DBNull)? "": (string)dataReader["OptionalNewsName"],
                    OptionalNewsDescription = dataReader["OptionalNewsDescription"].GetType() == typeof(DBNull)? "": (string)dataReader["OptionalNewsDescription"],
                    OptionalNewsPrice = dataReader["OptionalNewsPrice"].GetType() == typeof(DBNull)? "": (string)dataReader["OptionalNewsPrice"]
                };
            }
            dataReader.Close();
            Connection.Close();
            return foundNewsItem;
        }//end GetNewsItem

        public bool AddNewsItem(NewsItem _newsItem)
        {
            bool confirmation = false;
            Connection.Open();
            //Create the command
            SqlCommand command = new SqlCommand {
                Connection = Connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "AddNewsItem"
            };

            //Create a parameter
            SqlParameter parameter = new SqlParameter {
                ParameterName = "@ImageType",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _newsItem.ImageType
            };
            //Add the parameter to the command's parameter list
            command.Parameters.Add(parameter);

            //Create a parameter
            parameter = new SqlParameter {
                ParameterName = "@ImageURL",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _newsItem.ImageURL
            };
            //Add the parameter to the command's parameter list
            command.Parameters.Add(parameter);

            //Create a parameter
            parameter = new SqlParameter {
                ParameterName = "@NewsItemLink",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _newsItem.NewsItemLink
            };
            //Add the parameter to the command's parameter list
            command.Parameters.Add(parameter);

            // //Create a parameter
            // parameter = new SqlParameter {
            //     ParameterName = "@IndexPosition",
            //     SqlDbType = SqlDbType.Int,
            //     Direction = ParameterDirection.Input,
            //     SqlValue = _newsItem.IndexPosition
            // };
            // //Add the parameter to the command's parameter list
            // command.Parameters.Add(parameter);

            //Create a parameter
            parameter = new SqlParameter {
                ParameterName = "@OptionalNewsName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _newsItem.OptionalNewsName
            };
            //Add the parameter to the command's parameter list
            command.Parameters.Add(parameter);

            //Create a parameter
            parameter = new SqlParameter {
                ParameterName = "@OptionalNewsDescription",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _newsItem.OptionalNewsDescription
            };
            //Add the parameter to the command's parameter list
            command.Parameters.Add(parameter);

            //Create a parameter
            parameter = new SqlParameter {
                ParameterName = "@OptionalNewsPrice",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _newsItem.OptionalNewsPrice
            };
            //Add the parameter to the command's parameter list
            command.Parameters.Add(parameter);
            
            // Execute the stored procedure
            command.ExecuteNonQuery();

            Connection.Close();
            confirmation = true;
            return confirmation;
        }//end Add News Item
    }//end class
}//end namespace
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CapstoneCustomerRelationsSystem.Domain;

namespace CapstoneCustomerRelationsSystem.TechnicalServices
{
    public class Products 
    {
        //ToDo: Replace this with config file.
        private SqlConnection Connection = new SqlConnection {
            ConnectionString =  Startup.CRSConnectionString
        };


        public Products() {

        }//end constructor

        public List<Product> GetProducts() {
            List<Product> productsToReturn = new List<Product>();
            Connection.Open();

            //Define the Command
            SqlCommand command = new SqlCommand {
                Connection = Connection,
                CommandText = "GetProducts",
                CommandType = CommandType.StoredProcedure
            };

            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.HasRows) {
                while (dataReader.Read()) {
                    productsToReturn.Add(
                        new Product {
                            ProductNumber = (int)dataReader["ProductNumber"],
                            ImageURL = (string)dataReader["ImageURL"],
                            // ProductLink = (string)dataReader["ProductLink"],
                            CompanyName = (string) dataReader["CompanyName"],
                            ProductName = dataReader["ProductName"].GetType() == typeof(DBNull)? "": (string)dataReader["ProductName"],
                            ProductDescription = dataReader["ProductDescription"].GetType() == typeof(DBNull)? "": (string)dataReader["ProductDescription"],
                            ProductPrice = Decimal.Round((decimal)dataReader["ProductPrice"], 2)
                        }
                    );
                }//end while loop
            }//end if the data reader has rows
            dataReader.Close();
            Connection.Close();
            return productsToReturn;
        }//end GetProducts

        internal bool DeleteProduct(int _productNumber)
        {
            bool success = false;

            SqlConnection connection = new SqlConnection {
                ConnectionString = Connection.ConnectionString
            };

            connection.Open();
            
            SqlCommand command = new SqlCommand {
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "DeleteProduct"
            };

            SqlParameter parameter = new SqlParameter {
                ParameterName = "@ProductNumber",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
                SqlValue = _productNumber
            };
            command.Parameters.Add(parameter);

            command.ExecuteNonQuery();

            connection.Close();

            success = true;
            return success;
        }//end DeleteProduct

        internal bool UpdateProduct(Product _productToUpdate)
        {
            bool success = false;

            SqlConnection connection = new SqlConnection {
                ConnectionString = Connection.ConnectionString
            };

            connection.Open();

            SqlCommand command = new SqlCommand {
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "UpdateProduct"
            };

            SqlParameter parameter = new SqlParameter {
                ParameterName = "@ProductNumber",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                SqlValue = _productToUpdate.ProductNumber
            };
            command.Parameters.Add(parameter);

            parameter = new SqlParameter {
                ParameterName = "@ImageURL",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _productToUpdate.ImageURL
            };
            command.Parameters.Add(parameter);

            // parameter = new SqlParameter {
            //     ParameterName = "@ProductLink",
            //     SqlDbType = SqlDbType.VarChar,
            //     Direction = ParameterDirection.Input,
            //     SqlValue = _productToUpdate.ProductLink
            // };
            // command.Parameters.Add(parameter);

            parameter = new SqlParameter {
                ParameterName = "@CompanyName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _productToUpdate.CompanyName
            };
            command.Parameters.Add(parameter);

            parameter = new SqlParameter {
                ParameterName = "@ProductName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _productToUpdate.ProductName ?? (object)DBNull.Value
            };
            command.Parameters.Add(parameter);

            parameter = new SqlParameter {
                ParameterName = "@ProductDescription",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _productToUpdate.ProductDescription ?? (object)DBNull.Value
            };
            command.Parameters.Add(parameter);

            parameter = new SqlParameter {
                ParameterName = "@ProductPrice",
                SqlDbType = SqlDbType.Money,
                Direction = ParameterDirection.Input,
                SqlValue = Decimal.Round(_productToUpdate.ProductPrice, 2)
            };
            command.Parameters.Add(parameter);

            command.ExecuteNonQuery();

            connection.Close();

            success = true;
            return success;
        }//end UpdateProduct

        internal bool AddProduct(Product _productToAdd)
        {
            bool success = false;

            SqlConnection connection = new SqlConnection {
                ConnectionString = Connection.ConnectionString
            };

            connection.Open();

            SqlCommand command = new SqlCommand {
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "AddProduct"
            };

            SqlParameter parameter = new SqlParameter {
                ParameterName = "@ImageURL",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _productToAdd.ImageURL
            };
            command.Parameters.Add(parameter);

            // parameter = new SqlParameter {
            //     ParameterName = "@ProductLink",
            //     SqlDbType = SqlDbType.VarChar,
            //     Direction = ParameterDirection.Input,
            //     SqlValue = _productToAdd.ProductLink
            // };
            // command.Parameters.Add(parameter);

            parameter = new SqlParameter {
                ParameterName = "@CompanyName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _productToAdd.CompanyName
            };
            command.Parameters.Add(parameter);

            parameter = new SqlParameter {
                ParameterName = "@ProductName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _productToAdd.ProductName ?? (object)DBNull.Value
            };
            command.Parameters.Add(parameter);

            parameter = new SqlParameter {
                ParameterName = "@ProductDescription",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _productToAdd.ProductDescription ?? (object)DBNull.Value
            };
            command.Parameters.Add(parameter);

            parameter = new SqlParameter {
                ParameterName = "@ProductPrice",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = Decimal.Round(_productToAdd.ProductPrice, 2)
            };
            command.Parameters.Add(parameter);

            command.ExecuteNonQuery();

            connection.Close();

            success = true;
            return success;
        }//end AddProduct

        internal Product GetProduct(int _productID)
        {
            Product foundProduct = null;

            SqlConnection connection = new SqlConnection {
                ConnectionString = Connection.ConnectionString
            };

            connection.Open();
            SqlCommand command = new SqlCommand {
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "GetProduct"
            };

            SqlParameter parameter = new SqlParameter {
                ParameterName = "@ProductNumber",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                SqlValue = _productID
            };
            command.Parameters.Add(parameter);

            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.HasRows) {
                dataReader.Read();
                foundProduct = new Product();
                foundProduct.ProductNumber = (int)dataReader["ProductNumber"];
                foundProduct.ImageURL = (string)dataReader["ImageURL"];
                // foundProduct.ProductLink = (string)dataReader["ProductLink"];
                foundProduct.CompanyName = (string)dataReader["CompanyName"];
                foundProduct.ProductName = dataReader["ProductName"] != DBNull.Value? (string)dataReader["ProductName"]:"";
                foundProduct.ProductDescription = dataReader["ProductDescription"] != DBNull.Value? (string)dataReader["ProductDescription"]:"";
                foundProduct.ProductPrice = Decimal.Round((decimal)dataReader["ProductPrice"], 2);
            }//end if datareader has rows

            connection.Close();

            return foundProduct;
        }//end GetProduct
    }//end class
}//end namespace
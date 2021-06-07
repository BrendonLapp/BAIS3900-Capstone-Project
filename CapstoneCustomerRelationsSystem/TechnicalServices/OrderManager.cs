using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CapstoneCustomerRelationsSystem.Domain.Models;

namespace CapstoneCustomerRelationsSystem.TechnicalServices
{
    public class OrderManager
    {
        private SqlConnection Connection { get; set; } = new SqlConnection {
            ConnectionString = Startup.CRSConnectionString
        };

        public List<Order> GetIncompletedOrders()
        {
            List<Order> Orders = new List<Order>();

            SqlConnection CapstoneDB = new SqlConnection();
            CapstoneDB.ConnectionString = Connection.ConnectionString;
            CapstoneDB.Open();

            SqlCommand GetIncompletedOrdersCommand = new SqlCommand
            {
                CommandText = "GetIncompletedOrders",
                CommandType = CommandType.StoredProcedure,
                Connection = CapstoneDB
            };

            SqlDataReader DataReader;
            DataReader = GetIncompletedOrdersCommand.ExecuteReader();

            if (DataReader.HasRows)
            {
                while (DataReader.Read())
                {
                    Order FoundOrder = new Order();

                    for (int index = 0; index < DataReader.FieldCount; index++)
                    {
                        FoundOrder.OrderID = int.Parse(DataReader["OrderID"].ToString());
                        FoundOrder.CustomerUserAccountNumber = int.Parse(DataReader["CustomerUserAccountNumber"].ToString());
                        //FoundOrder.EmployeeUserAccountNumber = DataReader["EmployeeUserAccountNumber"].ToString() == null ? 0 : int.Parse(DataReader["EmployeeUserAccountNumber"].ToString());
                        FoundOrder.OrderStatus = DataReader["OrderStatus"].ToString();
                        FoundOrder.GST = decimal.Parse(DataReader["GST"].ToString());
                        FoundOrder.SubTotal = decimal.Parse(DataReader["SubTotal"].ToString());
                        FoundOrder.Total = decimal.Parse(DataReader["Total"].ToString());
                        FoundOrder.PlacedDate = DateTime.Parse(DataReader["PlacedDate"].ToString());
                        FoundOrder.CustomerName = DataReader["CustomerName"].ToString();
                        FoundOrder.CapstoneLocation = DataReader["RequestedStore"].ToString();
                        //FoundOrder.EmployeeName = DataReader["EmployeeName"].ToString();
                    }

                    Orders.Add(FoundOrder);
                }
            }

            DataReader.Close();
            CapstoneDB.Close();

            return Orders;

        }//End FindIncompleteOrders

        public Order GetOrdersByOrderID(int OrderID)
        {
            Order FoundOrder = new Order();

            SqlConnection CapstoneDB = new SqlConnection();
            CapstoneDB.ConnectionString = Connection.ConnectionString;
            CapstoneDB.Open();

            SqlCommand GetOrderByID = new SqlCommand
            {
                CommandText = "GetOrdersByOrderID",
                CommandType = CommandType.StoredProcedure,
                Connection = CapstoneDB
            };

            SqlParameter OrderIDParameter = new SqlParameter
            {
                ParameterName = "OrderID",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
                SqlValue = OrderID
            };
            GetOrderByID.Parameters.Add(OrderIDParameter);

            SqlDataReader DataReader;

            DataReader = GetOrderByID.ExecuteReader();

            if (DataReader.HasRows)
            {
                while (DataReader.Read())
                {
                    for (int index = 0; index < DataReader.FieldCount; index++)
                    {
                        FoundOrder.OrderID = int.Parse(DataReader["OrderID"].ToString());
                        FoundOrder.CustomerUserAccountNumber = int.Parse(DataReader["CustomerUserAccountNumber"].ToString());
                        FoundOrder.Email = DataReader["Email"].ToString();
                        FoundOrder.PhoneNumber = DataReader["PhoneNumber"].ToString();
                        //FoundOrder.EmployeeUserAccountNumber = int.Parse(DataReader["EmployeeUserAccountNumber"].ToString()) == null ? 0 : int.Parse(DataReader["EmployeeUserAccountNumber"].ToString());
                        FoundOrder.OrderStatus = DataReader["OrderStatus"].ToString();
                        FoundOrder.GST = decimal.Parse(DataReader["GST"].ToString());
                        FoundOrder.SubTotal = decimal.Parse(DataReader["SubTotal"].ToString());
                        FoundOrder.Total = decimal.Parse(DataReader["Total"].ToString());
                        FoundOrder.PlacedDate = DateTime.Parse(DataReader["PlacedDate"].ToString());
                        FoundOrder.CustomerName = DataReader["CustomerName"].ToString();
                        FoundOrder.CapstoneLocation = DataReader["RequestedStore"].ToString();
                        //FoundOrder.EmployeeName = DataReader["EmployeeName"].ToString();
                    }
                }
            }

            DataReader.Close();
            CapstoneDB.Close();

            return FoundOrder;
        }//End GetOrdersByOrderID

        public List<Order> GetOrdersByStatus(string OrderStatus)
        {
            List<Order> Orders = new List<Order>();

            SqlConnection CapstoneDB = new SqlConnection();
            CapstoneDB.ConnectionString = Connection.ConnectionString;
            CapstoneDB.Open();

            SqlCommand GetOrdersByStatusCommand = new SqlCommand
            {
                CommandText = "GetOrdersByStatus",
                CommandType = CommandType.StoredProcedure,
                Connection = CapstoneDB
            };

            SqlParameter OrderStatusParameter = new SqlParameter
            {
                ParameterName = "OrderStatus",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.VarChar,
                SqlValue = OrderStatus
            };
            GetOrdersByStatusCommand.Parameters.Add(OrderStatusParameter);

            SqlDataReader DataReader;
            DataReader = GetOrdersByStatusCommand.ExecuteReader();

            if (DataReader.HasRows)
            {
                while (DataReader.Read())
                {
                    Order FoundOrder = new Order();

                    for (int index = 0; index < DataReader.FieldCount; index++)
                    {
                        FoundOrder.OrderID = int.Parse(DataReader["OrderID"].ToString());
                        FoundOrder.CustomerUserAccountNumber = int.Parse(DataReader["CustomerUserAccountNumber"].ToString());
                        //FoundOrder.EmployeeUserAccountNumber = int.Parse(DataReader["EmployeeUserAccountNumber"].ToString()) == null ? 0 : int.Parse(DataReader["EmployeeUserAccountNumber"].ToString());
                        FoundOrder.OrderStatus = DataReader["OrderStatus"].ToString();
                        FoundOrder.GST = decimal.Parse(DataReader["GST"].ToString());
                        FoundOrder.SubTotal = decimal.Parse(DataReader["SubTotal"].ToString());
                        FoundOrder.Total = decimal.Parse(DataReader["Total"].ToString());
                        FoundOrder.PlacedDate = DateTime.Parse(DataReader["PlacedDate"].ToString());
                        FoundOrder.CapstoneLocation = DataReader["RequestedStore"].ToString();
                    }

                    Orders.Add(FoundOrder);
                }
            }

            DataReader.Close();
            CapstoneDB.Close();

            return Orders;
        }//End GetOrdersByStatus

        public int AddOrder(Order NewOrder)
        {
            SqlConnection CapstoneDB = new SqlConnection();
            CapstoneDB.ConnectionString = Connection.ConnectionString;
            CapstoneDB.Open();

            SqlCommand AddOrderCommand = new SqlCommand
            {
                CommandText = "AddOrder",
                CommandType = CommandType.StoredProcedure,
                Connection = CapstoneDB
            };

            SqlParameter CustomerUserAccountNumberParameter = new SqlParameter
            {
                ParameterName = "CustomerUserAccountNumber",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
                SqlValue = NewOrder.CustomerUserAccountNumber
            };
            AddOrderCommand.Parameters.Add(CustomerUserAccountNumberParameter);

            SqlParameter PlacedDateParameter = new SqlParameter
            {
                ParameterName = "PlacedDate",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Date,
                SqlValue = NewOrder.PlacedDate
            };
            AddOrderCommand.Parameters.Add(PlacedDateParameter);

            SqlParameter OrderStatusParameter = new SqlParameter
            {
                ParameterName = "OrderStatus",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.VarChar,
                SqlValue = NewOrder.OrderStatus
            };
            AddOrderCommand.Parameters.Add(OrderStatusParameter);

            SqlParameter GSTParameter = new SqlParameter
            {
                ParameterName = "GST",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Money,
                SqlValue = NewOrder.GST
            };
            AddOrderCommand.Parameters.Add(GSTParameter);

            SqlParameter SubTotalParameter = new SqlParameter
            {
                ParameterName = "SubTotal",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Money,
                SqlValue = NewOrder.SubTotal
            };
            AddOrderCommand.Parameters.Add(SubTotalParameter);

            SqlParameter TotalParameter = new SqlParameter
            {
                ParameterName = "Total",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Money,
                SqlValue = NewOrder.Total
            };
            AddOrderCommand.Parameters.Add(TotalParameter);

            SqlParameter CapstoneLocationParameter = new SqlParameter
            {
                ParameterName = "CapstoneLocation",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.VarChar,
                SqlValue = NewOrder.CapstoneLocation
            };
            AddOrderCommand.Parameters.Add(CapstoneLocationParameter);

            int OrderID = Convert.ToInt32(AddOrderCommand.ExecuteScalar().ToString());

            CapstoneDB.Close();
            return OrderID;
        }//End AddOrder

        public bool UpdateOrder(Order OrderDetails)
        {
            bool Success;

            SqlConnection CapstoneDB = new SqlConnection();
            CapstoneDB.ConnectionString = Connection.ConnectionString;
            CapstoneDB.Open();

            SqlCommand UpdateOrder = new SqlCommand
            {
                CommandText = "UpdateOrder",
                CommandType = CommandType.StoredProcedure,
                Connection = CapstoneDB
            };

            SqlParameter OrderIDParameter = new SqlParameter
            {
                ParameterName = "OrderID",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
                SqlValue = OrderDetails.OrderID
            };
            UpdateOrder.Parameters.Add(OrderIDParameter);

            SqlParameter OrderStatusParameter = new SqlParameter
            {
                ParameterName = "OrderStatus",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.VarChar,
                SqlValue = OrderDetails.OrderStatus
            };
            UpdateOrder.Parameters.Add(OrderStatusParameter);

            SqlParameter EmployeeUserAccountNumberParameter = new SqlParameter
            {
                ParameterName = "EmployeeUserAccountNumber",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.VarChar,
                SqlValue = OrderDetails.EmployeeUserAccountNumber
            };
            UpdateOrder.Parameters.Add(EmployeeUserAccountNumberParameter);

            SqlParameter TaxParameter = new SqlParameter
            {
                ParameterName = "Tax",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Money,
                SqlValue = OrderDetails.GST
            };
            UpdateOrder.Parameters.Add(TaxParameter);

            SqlParameter SubTotalParameter = new SqlParameter
            {
                ParameterName = "SubTotal",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Money,
                SqlValue = OrderDetails.SubTotal
            };
            UpdateOrder.Parameters.Add(SubTotalParameter);

            SqlParameter TotalParameter = new SqlParameter
            {
                ParameterName = "Total",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Money,
                SqlValue = OrderDetails.Total
            };
            UpdateOrder.Parameters.Add(TotalParameter);

            UpdateOrder.ExecuteNonQuery();
            CapstoneDB.Close();

            Success = true;
            return Success;
        }//End UpdateOrderStatus

        public bool UpdateOrder(Order Order, string OrderStatus, DateTime CompletedDate)
        {
            bool Success;

            SqlConnection CapstoneDB = new SqlConnection();
            CapstoneDB.ConnectionString = Connection.ConnectionString;
            CapstoneDB.Open();

            SqlCommand UpdateOrder = new SqlCommand
            {
                CommandText = "UpdateOrderAfterSale",
                CommandType = CommandType.StoredProcedure,
                Connection = CapstoneDB
            };

            SqlParameter OrderIDParameter = new SqlParameter
            {
                ParameterName = "OrderID",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
                SqlValue = Order.OrderID
            };
            UpdateOrder.Parameters.Add(OrderIDParameter);

            SqlParameter OrderStatusParameter = new SqlParameter
            {
                ParameterName = "OrderStatus",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.VarChar,
                SqlValue = OrderStatus
            };
            UpdateOrder.Parameters.Add(OrderStatusParameter);

            SqlParameter TaxParameter = new SqlParameter
            {
                ParameterName = "Tax",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Money,
                SqlValue = Order.GST
            };
            UpdateOrder.Parameters.Add(TaxParameter);

            SqlParameter SubTotalParameter = new SqlParameter
            {
                ParameterName = "SubTotal",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Money,
                SqlValue = Order.SubTotal
            };
            UpdateOrder.Parameters.Add(SubTotalParameter);

            SqlParameter TotalParameter = new SqlParameter
            {
                ParameterName = "Total",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Money,
                SqlValue = Order.Total
            };
            UpdateOrder.Parameters.Add(TotalParameter);

            SqlParameter CompletedDateParameter = new SqlParameter
            {
                ParameterName = "CompletedDate",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.VarChar,
                SqlValue = CompletedDate
            };
            UpdateOrder.Parameters.Add(CompletedDateParameter);

            UpdateOrder.ExecuteNonQuery();
            CapstoneDB.Close();

            Success = true;
            return Success;
        }//End UpdateOrderStatus

        public bool CompleteOrder(Order OrderDetails)
        {
            bool Success;

            SqlConnection CapstoneDB = new SqlConnection();
            CapstoneDB.ConnectionString = Connection.ConnectionString;
            CapstoneDB.Open();

            SqlCommand UpdateOrder = new SqlCommand
            {
                CommandText = "CompleteOrder",
                CommandType = CommandType.StoredProcedure,
                Connection = CapstoneDB
            };

            SqlParameter OrderIDParameter = new SqlParameter
            {
                ParameterName = "OrderID",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
                SqlValue = OrderDetails.OrderID
            };
            UpdateOrder.Parameters.Add(OrderIDParameter);

            SqlParameter OrderStatusParameter = new SqlParameter
            {
                ParameterName = "OrderStatus",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.VarChar,
                SqlValue = OrderDetails.OrderStatus
            };
            UpdateOrder.Parameters.Add(OrderStatusParameter);

            SqlParameter EmployeeUserAccountNumberParameter = new SqlParameter
            {
                ParameterName = "EmployeeUserAccountNumber",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.VarChar,
                SqlValue = OrderDetails.EmployeeUserAccountNumber
            };
            UpdateOrder.Parameters.Add(EmployeeUserAccountNumberParameter);

            SqlParameter CompletedDateParameter = new SqlParameter
            {
                ParameterName = "OrderStatus",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.VarChar,
                SqlValue = OrderDetails.CompletedDate
            };
            UpdateOrder.Parameters.Add(CompletedDateParameter);

            UpdateOrder.ExecuteNonQuery();
            CapstoneDB.Close();

            Success = true;
            return Success;
        }//End CompleteOrder

        public List<OrderItem> FindOrderItems(int OrderID)
        {
            List<OrderItem> OrderItems = new List<OrderItem>();

            SqlConnection CapstoneDB = new SqlConnection();
            CapstoneDB.ConnectionString = Connection.ConnectionString;
            CapstoneDB.Open();

            SqlCommand GetOrderItems = new SqlCommand
            {
                CommandText = "GetOrderItems",
                CommandType = CommandType.StoredProcedure,
                Connection = CapstoneDB
            };

            SqlParameter OrderIDParameter = new SqlParameter
            {
                ParameterName = "OrderID",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
                SqlValue = OrderID
            };
            GetOrderItems.Parameters.Add(OrderIDParameter);

            SqlDataReader DataReader;
            DataReader = GetOrderItems.ExecuteReader();

            if (DataReader.HasRows)
            {
                while (DataReader.Read())
                {
                    OrderItem Item = new OrderItem();

                    for (int index = 0; index < DataReader.FieldCount; index++)
                    {
                        Item.OrderItemID = int.Parse(DataReader["OrderItemID"].ToString());
                        if (DataReader["ProductID"].ToString() == null || DataReader["ProductID"].ToString() == "")
                        {
                            Item.SharedID = DataReader["CardID"].ToString();
                        }
                        else
                        {
                            Item.SharedID = DataReader["ProductID"].ToString();
                        }
                        Item.QuantityOnHand = int.TryParse(DataReader["QuantityOnHand"].ToString(), out var tempVal) ? tempVal : (int?)null;
                        Item.QuantityRequested = int.Parse(DataReader["QuantityRequested"].ToString());
                        Item.LineItemPrice = decimal.Parse(DataReader["LineItemPrice"].ToString());
                    }

                    OrderItems.Add(Item);
                }
            }

            DataReader.Close();
            CapstoneDB.Close();

            return OrderItems;
        }//End FindOrderItems

        public bool AddCardOrderItem(OrderItem Item)
        {
            bool Success;

            SqlConnection CapstoneDB = new SqlConnection();
            CapstoneDB.ConnectionString = Connection.ConnectionString;
            CapstoneDB.Open();

            SqlCommand AddOrderItems = new SqlCommand
            {
                CommandText = "AddCardToOrderItem",
                CommandType = CommandType.StoredProcedure,
                Connection = CapstoneDB
            };

            SqlParameter OrderIDParameter = new SqlParameter
            {
                ParameterName = "OrderID",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
                SqlValue = Item.OrderID
            };
            AddOrderItems.Parameters.Add(OrderIDParameter);

            SqlParameter CardIDParameter = new SqlParameter
            {
                ParameterName = "CardID",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.VarChar,
                SqlValue = Item.CardID
            };
            AddOrderItems.Parameters.Add(CardIDParameter);

            SqlParameter QuantityRequestedParameter = new SqlParameter
            {
                ParameterName = "QuantityRequested",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
                SqlValue = Item.QuantityRequested
            };
            AddOrderItems.Parameters.Add(QuantityRequestedParameter);

            SqlParameter LineItemPriceParameter = new SqlParameter
            {
                ParameterName = "LineItemPrice",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Money,
                SqlValue = Item.LineItemPrice
            };
            AddOrderItems.Parameters.Add(LineItemPriceParameter);

            AddOrderItems.ExecuteNonQuery();

            Success = true;
            return Success;
        }//End AddCardOrderItem

        public bool AddProductOrderItem(OrderItem Item)
        {
            bool Success;

            SqlConnection CapstoneDB = new SqlConnection();
            CapstoneDB.ConnectionString = Connection.ConnectionString;
            CapstoneDB.Open();

            SqlCommand AddOrderItems = new SqlCommand
            {
                CommandText = "AddProductToOrderItem",
                CommandType = CommandType.StoredProcedure,
                Connection = CapstoneDB
            };

            SqlParameter OrderIDParameter = new SqlParameter
            {
                ParameterName = "OrderID",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
                SqlValue = Item.OrderID
            };
            AddOrderItems.Parameters.Add(OrderIDParameter);

            SqlParameter ProductIDParameter = new SqlParameter
            {
                ParameterName = "ProductID",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
                SqlValue = Item.ProductID
            };
            AddOrderItems.Parameters.Add(ProductIDParameter);

            SqlParameter QuantityRequestedParameter = new SqlParameter
            {
                ParameterName = "QuantityRequested",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
                SqlValue = Item.QuantityRequested
            };
            AddOrderItems.Parameters.Add(QuantityRequestedParameter);

            SqlParameter LineItemPriceParameter = new SqlParameter
            {
                ParameterName = "LineItemPrice",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Money,
                SqlValue = Item.LineItemPrice
            };
            AddOrderItems.Parameters.Add(LineItemPriceParameter);

            AddOrderItems.ExecuteNonQuery();

            Success = true;
            return Success;
        }//End AddProductOrderItem

        public bool UpdateOrderItem(OrderItem Item)
        {
            bool Success;

            SqlConnection CapstoneDB = new SqlConnection();
            CapstoneDB.ConnectionString = Connection.ConnectionString;
            CapstoneDB.Open();

            SqlCommand UpdateOrderItemCommand = new SqlCommand
            {
                CommandText = "UpdateOrderItem",
                CommandType = CommandType.StoredProcedure,
                Connection = CapstoneDB
            };

            SqlParameter OrderItemIDParameter = new SqlParameter
            {
                ParameterName = "OrderItemID",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
                SqlValue = Item.OrderItemID
            };
            UpdateOrderItemCommand.Parameters.Add(OrderItemIDParameter);

            SqlParameter QuantityOnHandParameter = new SqlParameter
            {
                ParameterName = "QuantityOnHand",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
                SqlValue = Item.QuantityOnHand
            };
            UpdateOrderItemCommand.Parameters.Add(QuantityOnHandParameter);

            UpdateOrderItemCommand.ExecuteNonQuery();

            CapstoneDB.Close();
            Success = true;
            return Success;
        }//End UpdateOrderItem

        public List<Order> GetOrdersByUserAccountNumber(int UserAccountNumber)
        {
            List<Order> Orders = new List<Order>();

            SqlConnection CapstoneDB = new SqlConnection();
            CapstoneDB.ConnectionString = Connection.ConnectionString;
            CapstoneDB.Open();

            SqlCommand GetOrdersByCustomer = new SqlCommand
            {
                CommandText = "GetOrdersByCustomer",
                CommandType = CommandType.StoredProcedure,
                Connection = CapstoneDB
            };

            SqlParameter UserAccountNumberParameter = new SqlParameter
            {
                ParameterName = "UserAccountNumber",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.VarChar,
                SqlValue = UserAccountNumber
            };
            GetOrdersByCustomer.Parameters.Add(UserAccountNumberParameter);

            SqlDataReader DataReader;
            DataReader = GetOrdersByCustomer.ExecuteReader();

            if (DataReader.HasRows)
            {
                while (DataReader.Read())
                {
                    Order FoundOrder = new Order();

                    for (int index = 0; index < DataReader.FieldCount; index++)
                    {
                        FoundOrder.OrderID = int.Parse(DataReader["OrderID"].ToString());
                        FoundOrder.OrderStatus = DataReader["OrderStatus"].ToString();
                        FoundOrder.GST = decimal.Parse(DataReader["GST"].ToString());
                        FoundOrder.SubTotal = decimal.Parse(DataReader["SubTotal"].ToString());
                        FoundOrder.Total = decimal.Parse(DataReader["Total"].ToString());
                        FoundOrder.PlacedDate = DateTime.Parse(DataReader["PlacedDate"].ToString());
                        if (!DateTime.TryParse(DataReader["CompletedDate"].ToString(), out DateTime result))
                        {
                            FoundOrder.CompletedDate = DateTime.Parse("01/01/1901");
                        }
                        else
                        {
                            FoundOrder.CompletedDate = DateTime.Parse(DataReader["CompletedDate"].ToString());
                        }
                        FoundOrder.CapstoneLocation = DataReader["RequestedStore"].ToString();
                    }

                    Orders.Add(FoundOrder);
                }
            }

            DataReader.Close();
            CapstoneDB.Close();

            return Orders;
        }//End GetOrdersByUserAccountNumber

    }//End Class
}

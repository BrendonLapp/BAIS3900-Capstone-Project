using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using CapstoneCustomerRelationsSystem.Domain;
using CapstoneCustomerRelationsSystem.Domain.Models;

namespace CapstoneCustomerRelationsSystem.TechnicalServices
{
    public class CapstoneInfoManager
    {
        private SqlConnection Connection = new SqlConnection
        {
            ConnectionString = Startup.CRSConnectionString
        };//end SqlConnection

        public CapstoneInfo GetCapstoneInfo(string _CapstoneStoreName)
        {
            Connection.Open();

            SqlCommand getCapstoneInfoCommand = new SqlCommand
            {
                Connection = Connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "GetCapstoneInfo"
            };

            SqlParameter getCapstoneStoreParameter = new SqlParameter
            { 
                ParameterName = "@CapstoneStoreName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _CapstoneStoreName
            };
            getCapstoneInfoCommand.Parameters.Add(getCapstoneStoreParameter);

            SqlDataReader sqlDataReader = getCapstoneInfoCommand.ExecuteReader();
            CapstoneInfo Capstoneinfo = new CapstoneInfo();

            if (sqlDataReader.HasRows)
            {
                sqlDataReader.Read();
                Capstoneinfo.CapstoneSunday = (string)sqlDataReader["CapstoneSunday"];
                Capstoneinfo.CapstoneMonday = (string)sqlDataReader["CapstoneMonday"];
                Capstoneinfo.CapstoneTuesday = (string)sqlDataReader["CapstoneTuesday"];
                Capstoneinfo.CapstoneWednesday = (string)sqlDataReader["CapstoneWednesday"];
                Capstoneinfo.CapstoneThursday = (string)sqlDataReader["CapstoneThursday"];
                Capstoneinfo.CapstoneFriday = (string)sqlDataReader["CapstoneFriday"];
                Capstoneinfo.CapstoneSaturday = (string)sqlDataReader["CapstoneSaturday"];
                Capstoneinfo.CapstoneHoliday = (string)sqlDataReader["CapstoneHoliday"];
                Capstoneinfo.PhoneNumber = (string)sqlDataReader["PhoneNumber"];
                Capstoneinfo.Email = (string)sqlDataReader["Email"];
                Capstoneinfo.Address = (string)sqlDataReader["Address"];
                Capstoneinfo.AddressLink = (string)sqlDataReader["AddressLink"];
            }
            sqlDataReader.Close();
            Connection.Close();

            return Capstoneinfo;
        }//end GetCapstoneInfo(string _CapstoneStoreName)

        public bool UpdateCapstoneInfo(CapstoneInfo _Capstoneinfo)
        {
            bool Success = false;

            Connection.Open();

            SqlCommand modifyCapstoneInfoCommand = new SqlCommand
            {
                Connection = Connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "ModifyCapstoneInfo"
            };

            SqlParameter CapstoneStoreParameter = new SqlParameter
            {
                ParameterName = "@CapstoneStoreName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _Capstoneinfo.CapstoneStoreName
            };
            modifyCapstoneInfoCommand.Parameters.Add(CapstoneStoreParameter);

            SqlParameter sundayParamter = new SqlParameter
            {
                ParameterName = "@CapstoneSunday",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _Capstoneinfo.CapstoneSunday
            };
            modifyCapstoneInfoCommand.Parameters.Add(sundayParamter);

            SqlParameter mondayParameter = new SqlParameter
            {
                ParameterName = "@CapstoneMonday",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _Capstoneinfo.CapstoneMonday
            };
            modifyCapstoneInfoCommand.Parameters.Add(mondayParameter);

            SqlParameter tuesdayParameter = new SqlParameter
            {
                ParameterName = "CapstoneTuesday",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _Capstoneinfo.CapstoneTuesday
            };
            modifyCapstoneInfoCommand.Parameters.Add(tuesdayParameter);

            SqlParameter wednesdayParameter = new SqlParameter
            {
                ParameterName = "@CapstoneWednesday",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _Capstoneinfo.CapstoneWednesday
            };
            modifyCapstoneInfoCommand.Parameters.Add(wednesdayParameter);

            SqlParameter thursdayParameter = new SqlParameter
            {
                ParameterName = "@CapstoneThursday",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _Capstoneinfo.CapstoneThursday
            };
            modifyCapstoneInfoCommand.Parameters.Add(thursdayParameter);

            SqlParameter fridayParameter = new SqlParameter
            {
                ParameterName = "@CapstoneFriday",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _Capstoneinfo.CapstoneFriday
            };
            modifyCapstoneInfoCommand.Parameters.Add(fridayParameter);

            SqlParameter saturdayParameter = new SqlParameter
            {
                ParameterName = "@CapstoneSaturday",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _Capstoneinfo.CapstoneSaturday
            };
            modifyCapstoneInfoCommand.Parameters.Add(saturdayParameter);

            SqlParameter holidayParameter = new SqlParameter
            {
                ParameterName = "@CapstoneHoliday",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _Capstoneinfo.CapstoneHoliday
            };
            modifyCapstoneInfoCommand.Parameters.Add(holidayParameter);

            SqlParameter phoneNumberParameter = new SqlParameter
            {
                ParameterName = "@PhoneNumber",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _Capstoneinfo.PhoneNumber
            };
            modifyCapstoneInfoCommand.Parameters.Add(phoneNumberParameter);

            SqlParameter emailParameter = new SqlParameter
            {
                ParameterName = "@Email",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _Capstoneinfo.Email
            };
            modifyCapstoneInfoCommand.Parameters.Add(emailParameter);

            SqlParameter addresssParameter = new SqlParameter
            {
                ParameterName = "@Address",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _Capstoneinfo.Address
            };
            modifyCapstoneInfoCommand.Parameters.Add(addresssParameter);

            SqlParameter addressLinkParameter = new SqlParameter
            {
                ParameterName = "@AddressLink",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _Capstoneinfo.AddressLink
            };
            modifyCapstoneInfoCommand.Parameters.Add(addressLinkParameter);

            modifyCapstoneInfoCommand.ExecuteNonQuery();

            Connection.Close();
            Success = true;
            return Success;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using CapstoneCustomerRelationsSystem.Domain;

namespace CapstoneCustomerRelationsSystem.TechnicalServices 
{
    public class UserAccounts 
    {
        private SqlConnection Connection { get; set; } = new SqlConnection {
            ConnectionString = Startup.CRSConnectionString
        };
        
        public UserAccounts() {

        }//end Constructor

        internal List<string> AuthenticateUser(string _userName, string _password) {
            //Get the user's salt before trying to hash the password.
            List<string> roles = new List<string>();
            byte[] salt = GetUserSalt(_userName);
            
            Connection.Open();
            SqlCommand command = new SqlCommand {
                Connection = Connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "Login"
            };

            SqlParameter parameter = new SqlParameter {
                ParameterName = "@UserName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _userName
            };
            command.Parameters.Add(parameter);

            parameter = new SqlParameter {
                ParameterName = "@Password",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = Convert.ToBase64String(
                    KeyDerivation.Pbkdf2(
                        password: _password,
                        salt: salt,
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 10000,
                        numBytesRequested: 256 / 8)
                    )
            };
            command.Parameters.Add(parameter);

            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.HasRows) {
                dataReader.Read();
                roles.Add((string)dataReader["RoleName"]);
            }
            
            dataReader.Close();
            Connection.Close();
            return roles;
        }//end AuthenticateUser

        internal UserAccount GetUserByAccountNumber(int userAccountNumber)
        {
            UserAccount foundUserAccount = null;
            SqlConnection connection = new SqlConnection
            {
                ConnectionString = Connection.ConnectionString
            };

            connection.Open();

            SqlCommand command = new SqlCommand
            {
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "GetUserByAccountNumber"
            };

            SqlParameter parameter = new SqlParameter
            {
                ParameterName = "UserAccountNumber",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                SqlValue = userAccountNumber
            };
            command.Parameters.Add(parameter);

            SqlDataReader dataReader = command.ExecuteReader();

            //If at least one record was found.
            if (dataReader.HasRows)
            {
                dataReader.Read();
                //Set foundUserAccount to equal the found records values
                foundUserAccount = new UserAccount();
                foundUserAccount.FirstName = (string)dataReader["FirstName"];
                foundUserAccount.LastName = (string)dataReader["LastName"];
                foundUserAccount.UserName = (string)dataReader["UserName"];
                foundUserAccount.Email = (string)dataReader["Email"];
                foundUserAccount.DCINumber = (int)dataReader["DCINumber"];
            }//end if the data reader has rows.

            dataReader.Close();
            connection.Close();

            return foundUserAccount;
        }

        internal List<Role> GetRoles()
        {
            List<Role> foundRoles = new List<Role>();
            SqlConnection connection = new SqlConnection() {
                ConnectionString = Connection.ConnectionString
            };

            connection.Open();

            SqlCommand command = new SqlCommand {
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "GetRoles"
            };

            SqlDataReader dataReader = command.ExecuteReader();

            Role roleToAdd;

            //If there are any roles found.
            if (dataReader.HasRows) {
                while(dataReader.Read()) {
                    roleToAdd = new Role();
                    roleToAdd.RoleNumber = (int)dataReader["RoleNumber"];
                    roleToAdd.RoleName = (string)dataReader["RoleName"];
                    roleToAdd.RoleDescription = (string)dataReader["RoleDescription"];
                    foundRoles.Add(roleToAdd);
                }
            }

            dataReader.Close();
            connection.Close();

            return foundRoles;
        }//end GetRoles

        internal bool UnAssignRole(int _selectedUserAccountNumberToRemove, int _selectedRoleNumberToRemove) {
            bool success = false;
            SqlConnection connection = new SqlConnection() {
                ConnectionString = Connection.ConnectionString
            };

            connection.Open();

            SqlCommand command = new SqlCommand {
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "UnAssignRole"
            };

            SqlParameter parameter = new SqlParameter {
                ParameterName = "@UserAccountNumber",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
                SqlValue = _selectedUserAccountNumberToRemove
            };
            command.Parameters.Add(parameter);

            parameter = new SqlParameter {
                ParameterName = "@RoleNumber",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
                SqlValue = _selectedRoleNumberToRemove
            };
            command.Parameters.Add(parameter);

            command.ExecuteNonQuery();

            connection.Close();

            success = true;
            return success;
        }//end UnAssignRole

        internal bool AssignRole(UserAccount _userAccount, Role _roleToAssign)
        {
            bool success = false;
            SqlConnection connection = new SqlConnection() {
                ConnectionString = Connection.ConnectionString
            };

            connection.Open();

            SqlCommand command = new SqlCommand() {
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "AssignRole"
            };

            SqlParameter parameter = new SqlParameter() {
                ParameterName = "@UserAccountNumber",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
                SqlValue = _userAccount.UserAccountNumber,
            };
            command.Parameters.Add(parameter);

            parameter = new SqlParameter() {
                ParameterName = "@RoleNumber",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
                SqlValue = _roleToAssign.RoleNumber
            };
            command.Parameters.Add(parameter);

            command.ExecuteNonQuery();

            connection.Close();

            success = true;
            return success;
        }//end AssignRole

        internal bool UpdateUserAccountPassword(string _userName, string _password)
        {
            bool success = false;

            SqlConnection connection = new SqlConnection {
                ConnectionString = Connection.ConnectionString
            };

            //salt
            byte[] salt = new byte[128 / 8];
            using (var _rng = RandomNumberGenerator.Create())
            {
                _rng.GetBytes(salt);
            }

            //Hashed
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: _password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            connection.Open();

            SqlCommand command = new SqlCommand {
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "UpdateUserAccountPassword"
            };

            SqlParameter parameter = new SqlParameter {
                ParameterName = "@UserName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _userName
            };
            command.Parameters.Add(parameter);
            
            parameter = new SqlParameter {
                ParameterName = "@Password",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = hashed
            };
            command.Parameters.Add(parameter);

            parameter = new SqlParameter {
                ParameterName = "@Salt",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = Convert.ToBase64String(salt)     
            };
            command.Parameters.Add(parameter);

            command.ExecuteNonQuery();

            //ToDo: Send a notification email to the User saying their password has been changed.

            connection.Close();

            success = true;
            return success;
        }//end UpdateUserAccountPassword

        internal bool UpdateUserAccount(UserAccount _accountToUpdate, string _previousUserName)
        {
            bool success = false;
            SqlConnection connection = new SqlConnection {
                ConnectionString = Connection.ConnectionString
            };

            connection.Open();

            SqlCommand command = new SqlCommand {
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "UpdateUserAccount"
            };

            SqlParameter parameter = new SqlParameter {
                ParameterName = "@FirstName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _accountToUpdate.FirstName
            };
            command.Parameters.Add(parameter);

            parameter = new SqlParameter {
                ParameterName = "@LastName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _accountToUpdate.LastName
            };
            command.Parameters.Add(parameter);

            parameter = new SqlParameter {
                ParameterName = "@PhoneNumber",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = Regex.Replace(_accountToUpdate.PhoneNumber, @"[^0-9]+", string.Empty)
            };
            command.Parameters.Add(parameter);

            parameter = new SqlParameter {
                ParameterName = "@UserName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _accountToUpdate.UserName
            };
            command.Parameters.Add(parameter);

            parameter = new SqlParameter {
                ParameterName = "@PreviousUserName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _previousUserName
            };
            command.Parameters.Add(parameter);

            parameter = new SqlParameter {
                ParameterName = "@Email",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _accountToUpdate.Email
            };
            command.Parameters.Add(parameter);

            parameter = new SqlParameter {
                ParameterName = "@DCINumber",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                SqlValue = _accountToUpdate.DCINumber
            };
            command.Parameters.Add(parameter);

            command.ExecuteNonQuery();

            connection.Close();
            
            success = true;
            return success;
        }//end UpdateUserAccount

        internal bool AddUserAccount(UserAccount _accountToAdd, string _password)
        {
            bool success = false;
            SqlConnection connection = new SqlConnection {
                ConnectionString = Connection.ConnectionString
            };

            connection.Open();

            //salt
            byte[] salt = new byte[128 / 8];
            using (var _rng = RandomNumberGenerator.Create())
            {
                _rng.GetBytes(salt);
            }

            //Hashed
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: _password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            SqlCommand command = new SqlCommand {
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "AddUserAccountAndAssignRole"
            };

            SqlParameter parameter = new SqlParameter {
                ParameterName = "@FirstName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _accountToAdd.FirstName
            };
            command.Parameters.Add(parameter);

            parameter = new SqlParameter {
                ParameterName = "@LastName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _accountToAdd.LastName
            };
            command.Parameters.Add(parameter);

            parameter = new SqlParameter {
                ParameterName = "@PhoneNumber",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = Regex.Replace(_accountToAdd.PhoneNumber, @"[^0-9]+", string.Empty)
            };
            command.Parameters.Add(parameter);

            parameter = new SqlParameter {
                ParameterName = "@UserName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _accountToAdd.UserName
            };
            command.Parameters.Add(parameter);

            parameter = new SqlParameter {
                ParameterName = "@Email",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _accountToAdd.Email
            };
            command.Parameters.Add(parameter);

            parameter = new SqlParameter {
                ParameterName = "@DCINumber",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                SqlValue = _accountToAdd.DCINumber
            };
            command.Parameters.Add(parameter);

            parameter = new SqlParameter {
                ParameterName = "@Password",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = hashed
            };
            command.Parameters.Add(parameter);

            parameter = new SqlParameter {
                ParameterName = "@Salt",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = Convert.ToBase64String(salt)     
            };
            command.Parameters.Add(parameter);

            parameter = new SqlParameter {
                ParameterName = "@RoleName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                //Hard Coding the Role to Customer here.
                SqlValue = "Customer"
            };
            command.Parameters.Add(parameter);

            // Execute the command.
            command.ExecuteNonQuery();

            connection.Close();

            // Returns successful if the command executed without errors. 
            success = true;

            return success;
        }//end AddUserAccount

        internal List<UserAccount> GetUserAccountsByUserName(string _username)
        {
            List<UserAccount> foundUserAccounts = new List<UserAccount>();
            SqlConnection connection = new SqlConnection {
                ConnectionString = Connection.ConnectionString
            };

            connection.Open();

            SqlCommand command = new SqlCommand {
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "GetUserAccounts"
            };

            SqlDataReader dataReader = command.ExecuteReader();
            
            UserAccount userAccountToAdd;
            // If any User Accounts were found
            if (dataReader.HasRows) {
                while(dataReader.Read()) {
                    //Only add UserAccounts that contain the searched username.
                    if (((string)dataReader["UserName"]).ToLower().Contains(_username)) {
                        userAccountToAdd = new UserAccount();
                        //Set userAccountToAdd to equal the found records values.
                        userAccountToAdd.UserAccountNumber = (int)dataReader["UserAccountNumber"];
                        userAccountToAdd.FirstName = (string)dataReader["FirstName"];
                        userAccountToAdd.LastName = (string)dataReader["LastName"];
                        userAccountToAdd.UserName = (string)dataReader["UserName"];
                        userAccountToAdd.Email = (string)dataReader["Email"];
                        userAccountToAdd.DCINumber = (int)dataReader["DCINumber"];
                        userAccountToAdd.PhoneNumber = (string)dataReader["PhoneNumber"];
                        userAccountToAdd.Roles = new List<string> {
                            (string)dataReader["RoleName"]
                        };
                        //Because a User Account can have multiple roles, we must go through the 
                        //rest of the found records with the same User Account Number and add their roles to foundUserAccount.
                        while(dataReader.Read() && userAccountToAdd.UserAccountNumber == ((int)dataReader["UserAccountNumber"])) {
                            userAccountToAdd.Roles.Add((string)dataReader["RoleName"]);
                        }
                        foundUserAccounts.Add(userAccountToAdd);
                    }
                }//end while data reader.Read
            }//end if datareader has rows

            dataReader.Close();
            connection.Close();

            return foundUserAccounts;
        }//end GetUserAccountsByUserName

        internal UserAccount GetUserAccountByUserName(string _username)
        {
            UserAccount foundUserAccount = null;
            SqlConnection connection = new SqlConnection {
                ConnectionString = Connection.ConnectionString
            };

            connection.Open();

            SqlCommand command = new SqlCommand {
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "GetUserAccountByUserName"
            };

            SqlParameter parameter= new SqlParameter {
                ParameterName = "@UserName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _username
            };
            command.Parameters.Add(parameter);

            SqlDataReader dataReader = command.ExecuteReader();

            //If at least one record was found.
            if (dataReader.HasRows) {
                dataReader.Read();
                //Set foundUserAccount to equal the found records values
                foundUserAccount = new UserAccount();
                foundUserAccount.FirstName = (string)dataReader["FirstName"];
                foundUserAccount.LastName = (string)dataReader["LastName"];
                foundUserAccount.UserName = (string)dataReader["UserName"];
                foundUserAccount.Email = (string)dataReader["Email"];
                foundUserAccount.DCINumber = (int)dataReader["DCINumber"];
                foundUserAccount.PhoneNumber = (string)dataReader["PhoneNumber"];
                foundUserAccount.UserAccountNumber = (int)dataReader["UserAccountNumber"];
                foundUserAccount.Roles = new List<string> {
                    (string)dataReader["RoleName"]
                };

                //Because a User Account can have multiple roles, we must go through the 
                //rest of the found records and their roles to foundUserAccount
                while(dataReader.Read()) {
                    foundUserAccount.Roles.Add((string)dataReader["RoleName"]);
                }
            }//end if the data reader has rows.

            dataReader.Close();
            connection.Close();

            return foundUserAccount;
        }//end GetUserAccountByUserName

        private byte[] GetUserSalt(string _userName) {
            byte[] salt = {};
            SqlConnection connectionGetUserSalt = new SqlConnection {
                ConnectionString = Startup.CRSConnectionString
            };
            connectionGetUserSalt.Open();
            SqlCommand command = new SqlCommand {
                Connection = connectionGetUserSalt,
                CommandType = CommandType.StoredProcedure,
                CommandText = "GetUserSalt"
            };

            SqlParameter parameter = new SqlParameter {
                ParameterName = "@UserName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                SqlValue = _userName
            };
            command.Parameters.Add(parameter);

            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.HasRows) {
                dataReader.Read();
                salt = Convert.FromBase64String((string)dataReader["Salt"]);
            }

            dataReader.Close();
            connectionGetUserSalt.Close();
            return salt;
        }//end GetUserSalt

        public int GetUserAccountNumber(string UserName)
        {
            int UserAccountNumber = 0;

            SqlConnection CapstoneDB = new SqlConnection();
            CapstoneDB.ConnectionString = Connection.ConnectionString;
            CapstoneDB.Open();

            SqlCommand GetUserAccountNumberCommand = new SqlCommand
            {
                CommandText = "GetUserAccountNumber",
                CommandType = CommandType.StoredProcedure,
                Connection = CapstoneDB
            };

            SqlParameter UserNameParameter = new SqlParameter
            {
                ParameterName = "UserName",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.VarChar,
                SqlValue = UserName
            };
            GetUserAccountNumberCommand.Parameters.Add(UserNameParameter);

            SqlDataReader DataReader = GetUserAccountNumberCommand.ExecuteReader();

            if (DataReader.HasRows)
            {
                while (DataReader.Read())
                {
                    for (int index = 0; index < DataReader.FieldCount; index++)
                    {
                        UserAccountNumber = int.Parse(DataReader["UserAccountNumber"].ToString());
                    }
                }
            }

            DataReader.Close();
            CapstoneDB.Close();

            return UserAccountNumber;
        }//End GetUserAccountNumber
    }//end class
}//end namespace
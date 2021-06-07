using System;
using System.Collections.Generic;
using CapstoneCustomerRelationsSystem.TechnicalServices;

namespace CapstoneCustomerRelationsSystem.Domain 
{
    public class CRS 
    {
        private UserAccounts UserAccountManager = new UserAccounts();
        private NewsItems NewsItemManager = new NewsItems();
        private Products ProductManager = new Products();
        private Configurations ConfiguationsManager = new Configurations();
        
        public CRS() {

        }//end Constructor

        #region Manage Login
        public List<string> Login(string _userName, string _password) {
            return UserAccountManager.AuthenticateUser(_userName, _password);
        }//end Login

        public int GetUserAccountNumber(string UserName)
        {
            int UserAccountNumber;
            UserAccounts UserAccountManager = new UserAccounts();
            UserAccountNumber = UserAccountManager.GetUserAccountNumber(UserName);
            return UserAccountNumber;
        }

        public UserAccount GetUserAccountByNumber(int UserAccountNumber)
        {
            UserAccount User = new UserAccount();
            UserAccounts UserAccountManager = new UserAccounts();
            User = UserAccountManager.GetUserByAccountNumber(UserAccountNumber);
            return User;
        }
        #endregion

        #region Manage User Accounts
        internal UserAccount RetrieveCurrentUserAccountByUserName(string _username)
        {
            return UserAccountManager.GetUserAccountByUserName(_username);
        }//end RetrieveCurrentUserAccountByUserName

        internal List<UserAccount> RetrieveUserAccountsByUserName(string _username)
        {
            return UserAccountManager.GetUserAccountsByUserName(_username);
        }//end RetrieveUserAccountsByUserName

        internal bool MakeUserAccount(UserAccount _accountToAdd, string _password)
        {
            return UserAccountManager.AddUserAccount(_accountToAdd, _password);
        }//end MakeUserAccount

        internal bool ModifyUserAccountPassword(string _userName, string _password)
        {
            return UserAccountManager.UpdateUserAccountPassword(_userName, _password);
        }//end ModifyUserAccountPassword

        internal bool ModifyUserAccount(UserAccount _accountToUpdate, string _previousUserName)
        {
            return UserAccountManager.UpdateUserAccount(_accountToUpdate, _previousUserName);
        }//end ModifyUserAccount

        internal List<Role> RetrieveRoles()
        {
            return UserAccountManager.GetRoles();
        }//end RetrieveRoles

        internal bool GiveRole(UserAccount _userAccount, Role _roleToGive)
        {
            bool success = false;
            UserAccount foundUserAccount = null;
            if ((foundUserAccount = UserAccountManager.GetUserAccountByUserName(_userAccount.UserName)) != null) {
                //If the found User Account does not already have the selected role.
                if (!foundUserAccount.Roles.Contains(_roleToGive.RoleName)) {
                    success = UserAccountManager.AssignRole(foundUserAccount, _roleToGive);
                }
            }
            return success;
        }//end GiveRole

        internal bool RemoveRole(int _selectedUserAccountNumberToRemove, int _selectedRoleNumberToRemove) {
            return UserAccountManager.UnAssignRole(_selectedUserAccountNumberToRemove, _selectedRoleNumberToRemove);
        }//end RemoveRole
        #endregion

        #region Manage News
        //Retrieves all News Items
        public List<NewsItem> RetrieveNewsItems() {            
            return NewsItemManager.GetNewsItems();
        }//end RetrieveNewsItems

        //Makes a new News Item
        public bool MakeNewsItem(NewsItem _newsItem) {
            return NewsItemManager.AddNewsItem(_newsItem);
        }//end MakeNewsItem

        internal NewsItem RetrieveNewsItem(int _newsItemNumber)
        {
            return NewsItemManager.GetNewsItem(_newsItemNumber);
        }//end RetrieveNewsItem

        internal bool ModifyNewsItem(NewsItem _newsItem)
        {
            return NewsItemManager.UpdateNewsItem(_newsItem);
        }//end ModifyNewsItem

        internal bool RemoveNewsItem(int _newsItemNumber) {
            return NewsItemManager.DeleteNewsItem(_newsItemNumber);
        }//end RemoveNewsItemNumber
        #endregion

        #region Manage Products
        public List<Product> RetrieveProducts() {
            return ProductManager.GetProducts();
        }//end RetrieveProducts

        internal Product RetrieveProduct(int _productID)
        {
            return ProductManager.GetProduct(_productID);
        }//end RetrieveProduct

        internal bool MakeProduct(Product _product)
        {
            return ProductManager.AddProduct(_product);
        }//end MakeProduct

        internal bool RemoveProduct(int _productNumber)
        {
            return ProductManager.DeleteProduct(_productNumber);
        }//end Remove Product

        internal bool ModifyProduct(Product _productToUpdate)
        {
            return ProductManager.UpdateProduct(_productToUpdate);
        }//end ModifyProduct
        #endregion

    }//end class
}//end namespace
///Author: James Desmarais
///Date: Apr 4, 2021
//Last Modified By: James Desmarais
//Last Modified Date: Apr 4, 2021
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapstoneCustomerRelationsSystem.Domain;

namespace CapstoneCustomerRelationsSystem.Pages 
{
    [Authorize(Policy = "RequiredSignedInUser")]
    public class UserAccountModel : PageModel 
    {
        private CRS RequestDirector = new CRS();
        public UserAccount CurrentUser { get; private set; }

        [BindProperty]
        public string FirstName { get; set; }
        [BindProperty]
        public string LastName { get; set; }
        [BindProperty]
        public string UserName { get; set; }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string PhoneNumber { get; set; }
        [BindProperty]
        public int DCINumber { get; set; }
        public decimal InStoreCredit { get; set; }

        [BindProperty]
        public string Submit { get; set; }

        public string InfoMessage { get; private set; } = "";
        public string WarningMessage { get; private set; } = "";
        public string DangerMessage { get; private set; } = "";

        public void OnGet() {
            InitializeUserAccount();
        }//end OnGet

        public async Task<IActionResult> OnPost() {
            IActionResult pageToReturn = Page();
            switch (Submit) {
                case "Update":
                    ValidateFields();
                    //Check if the form is valid
                    if (ModelState.IsValid) {
                        UserAccount accountToSend = new UserAccount();
                        accountToSend.FirstName = FirstName;
                        accountToSend.LastName = LastName;
                        accountToSend.UserName = UserName;
                        accountToSend.Email = Email;
                        accountToSend.PhoneNumber = PhoneNumber;
                        accountToSend.DCINumber = DCINumber;

                        try {
                            if (RequestDirector.ModifyUserAccount(accountToSend, User.Identity.Name)) {
                                InfoMessage = "The user account was updated successfully!";
                                if (User.Identity.Name != UserName) {
                                    pageToReturn = RedirectToPage("/Logout");
                                }
                            } else {
                                DangerMessage = "Account update failed, please try again later.";
                            }                     
                        } catch (Exception _ex) {
                            //If this is an sql exception.
                            if (_ex.GetType() == typeof(System.Data.SqlClient.SqlException)) {
                                if (_ex.Message.Contains("Violation of UNIQUE KEY constraint 'UNIQUE_UserName'.")) {
                                    ModelState.AddModelError("UserName", "*That Username already exists, please enter a new one.");
                                }
                            }
                        }//end catch
                    }//end if modelstate is valid

                    break;
                default:
                    break;
            }//end Submit switch
            if (User.Identity.Name == UserName) {
                InitializeUserAccount();
            }
            return pageToReturn;
        }//end OnPost

        private void InitializeUserAccount() {
            CurrentUser = RequestDirector.RetrieveCurrentUserAccountByUserName(User.Identity.Name);
            FirstName = CurrentUser.FirstName;
            LastName = CurrentUser.LastName;
            UserName = CurrentUser.UserName;
            Email = CurrentUser.Email;
            PhoneNumber = CurrentUser.PhoneNumber;
            DCINumber = CurrentUser.DCINumber;
            // InStoreCredit = CurrentUser.InStoreCredit;
        }//end InitializeUserAccount

        private void ValidateFields() {
            if (FirstName == null || FirstName == "") {
                ModelState.AddModelError("FirstName", "*The First Name field is required.");
            }
            if (LastName == null || LastName == "") {
                ModelState.AddModelError("LastName", "*The Last Name field is required.");
            }
            if (UserName == null || UserName == "") {
                ModelState.AddModelError("UserName", "*The User Name field is required.");
            }
            if (Email == null || Email == "") {
                ModelState.AddModelError("Email", "*The Email field is required.");
            }
        }//end ValidateFields
        
    }//end class
}//end namespace
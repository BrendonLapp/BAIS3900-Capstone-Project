using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapstoneCustomerRelationsSystem.Domain;

namespace CapstoneCustomerRelationsSystem.Pages 
{
    //ToDo: Send a confirmation email.
    public class CreateUserAccountModel : PageModel 
    {
        private CRS RequestDirector = new CRS();

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
        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public string ConfirmPassword { get; set; }

        [BindProperty]
        public string Submit { get; set; }

        public string InfoMessage { get; private set; } = "";
        public string WarningMessage { get; private set; } = "";
        public string DangerMessage { get; private set; } = "";


        public void OnGet() {

        }//end OnGet

        public void OnPost() {
            switch (Submit) {
                case "CreateAccount":
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
                            if (RequestDirector.MakeUserAccount(accountToSend, Password)) {
                                InfoMessage = "The user account was created successfully!";
                            } else {
                                DangerMessage = "Account creation failed, please try again later.";
                            }                     
                        } catch (Exception _ex) {
                            //If this is an sql exception.
                            if (_ex.GetType() == typeof(System.Data.SqlClient.SqlException)) {
                                if (_ex.Message.Contains("Violation of UNIQUE KEY constraint 'UNIQUE_UserName'.")) {
                                    ModelState.AddModelError("UserName", "*That Username already exists, please enter a new one.");
                                }
                            }
                        }
                    }
                    break;
                default: 
                    break;
            }
        }//end OnPost

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
            if (Password == null || Password == "") {
                ModelState.AddModelError("Password", "*The Password field is required.");
            } else {
                if (Password.Length < 6 || Password.Length > 30) {
                    ModelState.AddModelError("Password", "*The Password is too short. Minimum 6 characters, maximum 30 characters.");
                }
                if (!Password.Any(char.IsUpper) || !Password.Any(char.IsLower) || !Password.Any(char.IsDigit)) {
                    ModelState.AddModelError("Password", "*The Password must contain a lowercase letter, an uppercase letter, and a number.");
                }
            }
            if (ConfirmPassword == null || ConfirmPassword == "") {
                ModelState.AddModelError("ConfirmPassword", "*The Confirm Password field is required.");
            }
            if (Password != ConfirmPassword) {
                ModelState.AddModelError("ConfirmPassword", "*The Password and Confirm Password fields must match.");
            }
        }//end ValidateFields

    }//end class
}//end namespace
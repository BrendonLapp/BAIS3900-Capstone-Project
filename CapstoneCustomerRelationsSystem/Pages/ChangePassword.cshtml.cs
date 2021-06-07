using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapstoneCustomerRelationsSystem.Domain;

namespace CapstoneCustomerRelationsSystem.Pages 
{
    //ToDo: Send a confirmation email.
    [Authorize(Policy = "RequiredSignedInUser")]
    public class ChangePasswordModel : PageModel 
    {
        private CRS RequestDirector = new CRS();

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
                case "UpdatePassword":
                    ValidateFields();
                    //Check if the form is valid
                    if (ModelState.IsValid) {
                        try {
                            if (RequestDirector.ModifyUserAccountPassword(User.Identity.Name, Password)) {
                                InfoMessage = "The user account password was updated successfully!";
                            } else {
                                DangerMessage = "Password update failed, please try again later.";
                            }                     
                        } catch (Exception _ex) {
                            //If this is an sql exception.
                            if (_ex.GetType() == typeof(System.Data.SqlClient.SqlException)) {
                                DangerMessage = _ex.Message;
                            }
                        }
                    }
                    break;
                default: 
                    break;
            }
        }//end OnPost

        private void ValidateFields() {
            if (Password == null || Password == "") {
                ModelState.AddModelError("Password", "*The Password field is required.");
            }
            if (ConfirmPassword == null || ConfirmPassword == "") {
                ModelState.AddModelError("ConfirmPassword", "*The Confirm Password field is required.");
            } else {
                if (Password.Length < 6 || Password.Length > 30) {
                    ModelState.AddModelError("Password", "*The Password is too short. Minimum 6 characters, maximum 30 characters.");
                }
                if (!Password.Any(char.IsUpper) || !Password.Any(char.IsLower) || !Password.Any(char.IsDigit)) {
                    ModelState.AddModelError("Password", "*The Password must contain a lowercase letter, an uppercase letter, and a number.");
                }
            }
            if (Password != ConfirmPassword) {
                ModelState.AddModelError("ConfirmPassword", "*The Password and Confirm Password fields must match.");
            }
        }//end ValidateFields

    }//end class
}//end namespace
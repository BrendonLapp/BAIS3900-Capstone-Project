///Author: James Desmarais
///Date: Apr 4, 2021
//Last Modified By: James Desmarais
//Last Modified Date: Apr 4, 2021
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapstoneCustomerRelationsSystem.Domain;

namespace CapstoneCustomerRelationsSystem.Pages 
{
    [Authorize(Roles = "Admin,Manager,Employee")]
    public class ManageUserAccountsModel : PageModel 
    {
        private CRS RequestDirector = new CRS();
        public List<UserAccount> FoundUserAccounts { get; private set; } = new List<UserAccount>();

        [BindProperty]
        public string SearchBar { get; set; }

        [BindProperty]
        public int SelectedUserAccountNumber { get; set; } = 0;

        [BindProperty]
        public int SelectedUserAccountNumberToRemove { get; set; } = 0;

        [BindProperty]
        public string SelectedRoleNameToRemove { get; set; } = "";

        [BindProperty]
        public int SelectedRoleType { get; set; }
        public List<Role> RoleTypes {get; private set; } = new List<Role>();

        [BindProperty]
        public string Submit { get; set; }

        public string InfoMessage { get; private set; } = "";
        public string WarningMessage { get; private set; } = "";
        public string DangerMessage { get; private set; } = "";

        public void OnGet() {
            PopulateRolesDropDownList();
        }//end OnGet

        public void OnPost() {
            PopulateRolesDropDownList ();
            switch (Submit) {
                case "Search":
                    ValidateFields();
                    if (ModelState.IsValid) {
                        FindUserAccounts(SearchBar.ToLower());
                    }
                    break;
                case "AddRole":
                    if (User.IsInRole("Admin")) {
                        ValidateFields();                                           
                        if (ModelState.IsValid) {
                            //Find the User Accounts
                            FindUserAccounts(SearchBar.ToLower());

                            //Check if a valid option was selected
                            if (SelectedRoleType <1 || SelectedRoleType > RoleTypes.Count) {
                                ModelState.AddModelError("RoleType","*Please select a role.");  
                            } 

                            if (ModelState.IsValid) {
                                //Assign the role. ToDo: Finish this.
                                if (RequestDirector.GiveRole(
                                    FoundUserAccounts.Single(_x => _x.UserAccountNumber == SelectedUserAccountNumber), 
                                    RoleTypes.Single(_x => _x.RoleNumber == SelectedRoleType))) {
                                    InfoMessage += "Add Role succeeded.";
                                } else {
                                    DangerMessage += "*Add Role Failed, please try again later.";
                                }
                                //Find the User Accounts again
                                FindUserAccounts(SearchBar.ToLower());
                            } else {
                                DangerMessage += "*Add Role failed, please select a valid role.";
                            }
                        }
                    }
                    break;
                case "RemoveRole":
                    ValidateFields();
                    if (ModelState.IsValid) {
                        //Find the User Accounts
                        FindUserAccounts(SearchBar.ToLower());

                        //Remove the Role from the User Account
                        try {
                            RequestDirector.RemoveRole(SelectedUserAccountNumberToRemove, 
                                RequestDirector.RetrieveRoles()
                                .Where(_role => _role.RoleName == SelectedRoleNameToRemove)
                                .Select(_role => _role.RoleNumber).First());
                        } catch(Exception _ex) {

                        }

                        //Find the User Accounts again
                        FindUserAccounts(SearchBar.ToLower());
                    }
                   break;
                default:
                    break;
            }//end Submit switch
        }//end OnPost

        private void FindUserAccounts(string _username) {
            FoundUserAccounts = RequestDirector.RetrieveUserAccountsByUserName(_username);
        }//end FindUserAccounts

        private void PopulateRolesDropDownList () {
            RoleTypes = RequestDirector.RetrieveRoles();
        }//end PopulateRolesDropDownList

        private void ValidateFields() {
            if (SearchBar == null || SearchBar == "") {
                ModelState.AddModelError("SearchBar", "*You must enter part of a Username before you can search.");
            }
        }//end ValidateFields
        
    }//end class
}//end namespace
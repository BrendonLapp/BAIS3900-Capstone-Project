using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CapstoneCustomerRelationsSystem.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CapstoneCustomerRelationsSystem.Pages 
{
    public class LoginModel : PageModel 
    {
        private CRS RequestDirector { get; set; } = new CRS();
        [BindProperty]
        public string UserName { get; set; }
        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public string Submit { get; set; }

        public async Task<IActionResult> OnPost() {
            IActionResult pageToReturn = Page();
            switch(Submit) {
                case "Sign In": 
                    if (UserName != null && Password != null) {
                        List<Claim> claims = new List<Claim>() {
                                new Claim(ClaimTypes.Name, UserName)
                            };

                        // string role;
                        List<string> roles;
                        
                        //If a user with roles was found.
                        if ((roles = RequestDirector.Login(UserName, Password)).Count > 0) {
                            //Set User.Name to equal UserName.
                            claims.Add(
                                new Claim(ClaimTypes.Name, UserName)
                            );

                            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            
                            //ToDo: Confirm if I'm seeing multiple roles or if I'm resetting the same role multiple times.
                            //If the latter, make it a list delmited by ','
                            //Foreach role
                            foreach(string _role in roles) {
                                //Set User.Role to equal the found role.
                                claimsIdentity.AddClaim(
                                    new Claim(ClaimTypes.Role, _role)
                                );
                            }//end foreach loop

                            AuthenticationProperties authProperties = 
                                new AuthenticationProperties {
                                    IsPersistent = true,
                                    IssuedUtc = DateTime.Now,
                            };

                            await HttpContext.SignInAsync (
                                CookieAuthenticationDefaults.AuthenticationScheme,
                                new ClaimsPrincipal(claimsIdentity),
                                authProperties
                            );

                            //If a page that requires authentication was selected,
                            //set pageToReturn to the chosen page. 
                            string chosenPage = Request.Query["ReturnUrl"];
                            if (chosenPage != null) {
                                if (chosenPage.Contains("/")) {
                                    //Grabs everything after the last /, in this case the directory.
                                    chosenPage = chosenPage.Substring(chosenPage.LastIndexOf("/"));
                                }
                                pageToReturn = RedirectToPage(chosenPage);
                            } else {
                                //else, return to the index page.
                                pageToReturn = RedirectToPage("./Index");
                            }
                        }
                    }
                    break;
                default:
                    break;
            }//end switch

            //Return to the chosen page.
            return pageToReturn;
        }//end OnPost
    }//end class
}//end namespace
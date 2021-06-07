using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using CapstoneCustomerRelationsSystem.Domain;
using CapstoneCustomerRelationsSystem.Domain.Models;

namespace CapstoneCustomerRelationsSystem.Pages
{
    public class ContactModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Your name is required.")]
        public string Name { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Your email is required.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "A subject title is required.")]
        public string Subject { get; set; }
        [BindProperty]
        [Display(Name="Message")]
        [Required(ErrorMessage = "A message to Capstone is required.")]
        public string EmailText { get; set; }
        [BindProperty]
        public string Submit { get; set; }
        public string SuccessMessage { get; set; }
        
        public void OnGet()
        {

        }//End OnGet

        public void OnPost()
        {
            if (Submit == "Submit")
            {
                bool Success;

                ContactController ContactController = new ContactController();

                Message Message = new Message
                {
                    FromName = Name,
                    FromEmail = Email,
                    ToName = "Capstone Comics and Games",
                    ToEmail = "CapstoneDemoForCapstone@gmail.com",
                    Subject = Subject + " From: " + "CapstoneDemoForCapstone@gmail.com",
                    Text = EmailText
                };

                Success = ContactController.SendEmail(Message);

                if (Success == true)
                {
                    SuccessMessage = "Thank you for contacting Capstone Comics and Games. We will respond to your Email soon.";
                    Name = "";
                    Email = "";
                    Subject = "";
                    EmailText = "";
                }
                else
                {
                    SuccessMessage = "Something went wrong with. Please try again.";
                }
            }
        }//End OnPost
    }
}

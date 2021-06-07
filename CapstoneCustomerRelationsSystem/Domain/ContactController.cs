using System.Collections.Generic;
using CapstoneCustomerRelationsSystem.Domain.Models;
using CapstoneCustomerRelationsSystem.TechnicalServices;

namespace CapstoneCustomerRelationsSystem.Domain
{
    public class ContactController
    {
        public bool SendEmail(Message IncomingMessage)
        {
            bool Success;

            EmailManager EmailManager = new EmailManager();

            Success = EmailManager.SendEmail(IncomingMessage);

            return Success;
        }//End SendEmail

        public List<Message> GetEmails()
        {
            List<Message> Inbox = new List<Message>();

            EmailManager EmailManager = new EmailManager();

            Inbox = EmailManager.RetrieveEmails();

            Inbox.Reverse();

            return Inbox;
        }//End GetEmails

        public bool SendEmailForOrder(Order OrderEmail, string Subject, string Text, string HTMLText)
        {
            bool Success;
            Message Email = new Message();
            UserAccount User = new UserAccount();
            EmailManager EmailManager = new EmailManager();
            CRS Request = new CRS();
            User = Request.GetUserAccountByNumber(OrderEmail.CustomerUserAccountNumber);

            Email.ToEmail = User.Email;
            Email.ToName = User.FirstName + " " + User.LastName;
            Email.Subject = Subject;
            Email.Text = Text;
            Email.HtmlText = HTMLText
                + "<br/>"
                + "<table>" 
                + "<thead style=\"text-align: left !important;\">"
                + "<tr>"
                + "<th style=\"width: 60%; margin-left: 0 !important; padding-left: 0 !important;\">" + "Name" + "</th>"
                + "<th style=\"width: 20%; margin-left: 0 !important; padding-left: 0 !important;\">" + "Quantity Requested" + "</th>"
                + "<th style=\"width: 15%; margin-left: 0 !important; padding-left: 0 !important;\">" + "Price Per Item" + "</th>"
                + "<th style=\"margin-left: 0 !important; padding-left: 0 !important;\">" + "Price" + "</th>"
                + "</tr>"
                + "</thead>"
                + "<tbody>";
            foreach (var Item in OrderEmail.OrderItems)
            {
                Email.Text = Email.Text + Item.Name + ": " + Item.QuantityRequested + " " + Item.LineItemPrice + " " + Item.QuantityRequested * Item.LineItemPrice;
                Email.HtmlText = Email.HtmlText
                    + "<tr>"
                    + "<td>" + Item.Name + "</td>"
                    + "<td>" + Item.QuantityRequested + "</td>"
                    + "<td>" + Item.LineItemPrice + "</td>"
                    + "<td>" + Item.LineItemPrice * Item.QuantityRequested + "</td>"
                    + "</tr>";
            }
            Email.HtmlText = Email.HtmlText
                + "</tbody>"
                + "</table>"
                + "<br/>"
                + "SubTotal: " + " " + OrderEmail.SubTotal
                + "<br/>"
                + "GST: " + " " + OrderEmail.GST
                + "<br/>"
                + "Total: " + " " + OrderEmail.Total;
            Email.FromEmail = "CapstoneDemoForCapstone@gmail.com";
            Email.FromName = "Capstone Comics and Games";

            Success = EmailManager.SendEmail(Email);

            return Success;
        }
    }
}

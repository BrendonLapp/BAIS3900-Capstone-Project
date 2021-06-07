using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using CapstoneCustomerRelationsSystem.Domain.Models;
using MailKit.Search;

namespace CapstoneCustomerRelationsSystem.TechnicalServices
{
    public class EmailManager
    {
        public bool SendEmail(Message IncomingMessage)
        {
            bool Confirmation;

            Configurations Config = new Configurations();
            string EmailAddress = Config.GetConfiguration("Email", "EmailAddress");
            string Password = Config.GetConfiguration("Email", "Password");

            try
            {
                var Message = new MimeMessage();
                Message.From.Add(new MailboxAddress(IncomingMessage.FromName, IncomingMessage.FromEmail));
                Message.To.Add(new MailboxAddress(IncomingMessage.ToName, IncomingMessage.ToEmail));
                Message.Subject = IncomingMessage.Subject;
                Message.Body = new TextPart("plain")
                {
                    Text = IncomingMessage.Text
                };
                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = IncomingMessage.HtmlText;
                bodyBuilder.TextBody = IncomingMessage.Text;
                Message.Body = bodyBuilder.ToMessageBody();

                using (var Client = new SmtpClient())
                {
                    Client.CheckCertificateRevocation = false;
                    //Client.Connect("smtp.gmail.com", 587, false);
                    Client.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                    //Client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTlsWhenAvailable);
                    Client.Authenticate(EmailAddress, Password);
                    Client.Send(Message);
                    Client.Disconnect(true);
                }

                Confirmation = true;
            }
            catch
            {
                Confirmation = false;
            }

            return Confirmation;
        }//End SendEmail

        public List<Message> RetrieveEmails()
        {
            List<Message> InboxMessages = new List<Message>();

            Configurations Config = new Configurations();
            string EmailAddress = Config.GetConfiguration("Email", "EmailAddress");
            string Password = Config.GetConfiguration("Email", "Password");

            using (var Client = new ImapClient())
            {
                Client.CheckCertificateRevocation = false;
                Client.Connect("imap.gmail.com", 993, true);
                Client.Authenticate(EmailAddress, Password);

                var Inbox = Client.Inbox;
                Inbox.Open(FolderAccess.ReadOnly);
                for (int index = 0; index < Inbox.Count; index++)
                {
                    var Email = Inbox.GetMessage(index);
                    Message Holding = new Message
                    {
                        FromEmail = Email.From.Mailboxes.FirstOrDefault().Address,
                        FromName = Email.From.Mailboxes.FirstOrDefault().Name,
                        Subject = Email.Subject,
                        Text = Email.TextBody
                    };
                    InboxMessages.Add(Holding);
                }

                Client.Disconnect(true);
            }

            return InboxMessages;
        }//End RetrieveEmails
    }
}

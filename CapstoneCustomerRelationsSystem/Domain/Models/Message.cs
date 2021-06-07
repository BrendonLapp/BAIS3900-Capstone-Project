namespace CapstoneCustomerRelationsSystem.Domain.Models
{
    public class Message
    {
        public string ToName { get; set; }
        public string ToEmail { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public string HtmlText { get; set; }
    }
}

namespace notification_service.Notifications.DTOS
{
    public class RequestSendMessage
    {
        public Guid IdMessage { get; set; }

        public string To { get; set; }

        public string Subject { get; set; }


        public string Body { get; set; }


        public string TypeService { get; set; }  // Email, SMS

        public DateTime CreatedAt { get; set; }

        public RequestSendMessage(string to, string subject, string body, string messageType)
        {
            To = to;
            Subject = subject;
            Body = body;
            TypeService = messageType;
            CreatedAt = DateTime.UtcNow;
            IdMessage = Guid.NewGuid();
        }

    }
}

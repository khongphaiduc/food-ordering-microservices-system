namespace notification_service.Notifications.DTOS
{
    public class NotificationDTOS
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string TypeService { get; set; } = "Email";
    }
}

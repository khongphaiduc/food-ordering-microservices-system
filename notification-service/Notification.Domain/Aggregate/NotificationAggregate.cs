using System.Runtime.CompilerServices;

namespace notification_service.Notification.Domain.Aggregate
{
    public class NotificationAggregate
    {
        public Guid Id { get; private set; }

        public Guid IdUser { get; private set; }

        public string TypeNotification { get; private set; } = null!;

        public string Recipient { get; private set; } = null!;

        public string Message { get; private set; } = null!;

        public string Status { get; private set; }

        public string ProvideResponse { get; private set; }

        public DateTime CreateAt { get; private set; }

        public DateTime UpdateAt { get; private set; }

        private NotificationAggregate()
        {

        }

        internal NotificationAggregate(Guid id, Guid idUser, string typeNotification, string recipient, string message, string status, string provideResponse, DateTime createAt, DateTime updateAt)
        {
            Id = id;
            IdUser = idUser;
            TypeNotification = typeNotification;
            Recipient = recipient;
            Message = message;
            Status = status;
            ProvideResponse = provideResponse;
            CreateAt = createAt;
            UpdateAt = updateAt;
        }


        public static NotificationAggregate CreateNewNotification(string TypeNotification, string Recipient, string Message, string status, string provide)
        {
            return new NotificationAggregate
            {
                Id = Guid.NewGuid(),
                TypeNotification = TypeNotification,
                Recipient = Recipient,
                Message = Message,
                Status = status,
                ProvideResponse = provide,
                CreateAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow
            };

        }


        public void UpdateStatusNotification(string status, string provide)
        {
            Status = status;
            ProvideResponse = provide;
            UpdateAt = DateTime.UtcNow;
        }


    }
}

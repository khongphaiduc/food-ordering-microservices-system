using notification_service.Notifications.DTOS;

namespace notification_service.Notification.Domain.Interface
{
    public interface INotificationRepository
    {
        Task<bool> AddNewRecordNotification(RequestSendMessage message);

    }
}

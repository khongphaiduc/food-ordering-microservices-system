using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace payment_service.PaymentService.Infastructure.NotificationRealTimeSignalR
{
    [Authorize(AuthenticationSchemes = "AccessToken")]
    public class NotificationPaidSusscessfully : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("UserId is NULL");
                Context.Abort();
                return;
            }

            Console.WriteLine($"User connected: {userId}");

            await base.OnConnectedAsync();
        }

    }
}

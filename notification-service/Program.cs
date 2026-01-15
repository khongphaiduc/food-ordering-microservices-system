using notification_service.Notifications.EmailSerivce;
using notification_service.Notifications.Services;
using notification_service.Worker.EmailWorker;

namespace notification_service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();


            builder.Services.AddSingleton<INotifications, Emails>(); 

            builder.Services.AddHostedService<EmailConsumer>();

            var app = builder.Build();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}

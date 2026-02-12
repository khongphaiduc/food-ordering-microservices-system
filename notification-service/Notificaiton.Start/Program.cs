using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using notification_service.Models;
using notification_service.Notification.Application.Services;
using notification_service.Notification.Domain.Interface;
using notification_service.Notification.Infastructure.Repositories;
using notification_service.Notification.Infastructure.Worker.EmailWorker;
using notification_service.Notifications.Services;

namespace notification_service.Notificaiton.Start
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotNetEnv.Env.Load();
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddDbContext<FoodNotificationDbContext>(options =>
            {

                options.UseNpgsql(builder.Configuration["URLSQL_NOTIFICATIONS"]);

            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("AccessToken", option =>
             {
            option.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key:AccessToken"]!))
            };
             });


            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

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

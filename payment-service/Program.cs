using Microsoft.EntityFrameworkCore;
using payment_service.PaymentService.API.gRPC;
using payment_service.PaymentService.Application.Services;
using payment_service.PaymentService.Infastructure.Models;
using payment_service.PaymentService.Infastructure.ServicesImplements;
using PayOS;

namespace payment_service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotNetEnv.Env.Load();
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();



            builder.Services.AddDbContext<FoodPaymentContext>(options =>
            {
                options.UseSqlServer(builder.Configuration["SQL"]);
            });


            builder.Services.AddSingleton<PayOSClient>(sp =>
            {
                return new PayOSClient(new PayOSOptions
                {
                    ClientId = builder.Configuration["ClientIDPayOS"],
                    ApiKey = builder.Configuration["ApiKeytPayOS"],
                    ChecksumKey = builder.Configuration["ChecksumKeyPOS"]

                });
            });

            builder.Services.AddScoped<ICreateNewPaymentOrder, CreateNewPaymentOrder>(); 
            
            builder.Services.AddGrpc();
            var app = builder.Build();

            app.MapGrpcService<CreatePaymentServers>();

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}

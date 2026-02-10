

using CartService.API.Protos;
using Microsoft.EntityFrameworkCore;
using order_service.OrderService.API.gRPC;
using order_service.OrderService.Appilcation.Services;
using order_service.OrderService.Domain.Interface;
using order_service.OrderService.Infastructure.Models;
using order_service.OrderService.Infastructure.Repository;
using order_service.OrderService.Infastructure.ServicesImplements;

namespace order_service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotNetEnv.Env.Load();
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<FoodOrderContext>(options =>
            {
                options.UseSqlServer(builder.Configuration["URLORDER"]);
            });


            builder.Services.AddGrpcClient<CartInforGrpc.CartInforGrpcClient>(options =>
            {
                options.Address = new Uri("https://localhost:7185");

            });

            builder.Services.AddScoped<IOrderRepository, OrderRepository>();

            builder.Services.AddScoped<ICreateNewOrder, CreateNewOrder>();

            builder.Services.AddScoped<GetInformationOfCart>();

            builder.Services.AddControllers();

            builder.Services.AddGrpc();
            var app = builder.Build();



            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

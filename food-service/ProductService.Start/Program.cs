using food_service.Models;
using food_service.ProductService.API.Middlwares;
using food_service.ProductService.Application.Interface;
using food_service.ProductService.Application.Service;
using food_service.ProductService.Domain.Interface;
using food_service.ProductService.Infastructure.BackgroundServices;
using food_service.ProductService.Infastructure.ImplementService;
using food_service.ProductService.Infastructure.ProducerRabbitMQ;
using food_service.ProductService.Infastructure.RedisService.RedisInterface;
using food_service.ProductService.Infastructure.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace food_service.ProductService.Start
{
    public class Program
    {
        public static void Main(string[] args)
        {

            DotNetEnv.Env.Load();

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<FoodProductsDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration["SQLFOOD_PRODUCTS"]!);
            });


            builder.Services.AddControllers();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

            });


            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IGetListProduct, GetListProduct>();
            builder.Services.AddScoped<IViewDetailProduct, ViewDetailProduct>();
            builder.Services.AddScoped<ICreateNewProduct, CreateNewProduct>();
            builder.Services.AddScoped<IUpdateCategory, UpdateCategory>();
            builder.Services.AddScoped<ICreateNewCategory, CreateNewCategory>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddSingleton<FoodProducer>();

            builder.Services.AddScoped<IOutBoxPatternProduct, OutBoxPatternProduct>();


            //redis 

            builder.Services.AddTransient<IRedisLockService, RedisLockService>();

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration["RedisAddress"];
                options.InstanceName = "FoodTrungDuc";
            });
            // Redis lock
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
              ConnectionMultiplexer.Connect(builder.Configuration["RedisAddress"]!));


            //backgroundSerivce
            builder.Services.AddHostedService<OutboxMessageProcessor>();
            var app = builder.Build();

            app.UseMiddleware<CustomGlobalException>();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

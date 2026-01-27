using food_service.ProductService.API.Middlwares;
using food_service.ProductService.Application.Interface;
using food_service.ProductService.Application.Service;
using food_service.ProductService.Domain.Interface;
using food_service.ProductService.Infastructure.BackgroundServices;
using food_service.ProductService.Infastructure.ImplementService;
using food_service.ProductService.Infastructure.MinIO;
using food_service.ProductService.Infastructure.Models;
using food_service.ProductService.Infastructure.ProducerRabbitMQ;
using food_service.ProductService.Infastructure.RedisService.RedisInterface;
using food_service.ProductService.Infastructure.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Minio;
using Serilog;
using StackExchange.Redis;

namespace food_service.ProductService.Start
{
    public class Program
    {
        public static void Main(string[] args)
        {

            DotNetEnv.Env.Load();

            var builder = WebApplication.CreateBuilder(args);

            ////serilog 
            builder.Host.UseSerilog((context, services, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext();
            });

            builder.Services.AddDbContext<FoodProductsDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration["SQLFOOD_PRODUCTS"]!);
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

            });


            builder.Services.AddRateLimiter(option =>
            {
                option.AddFixedWindowLimiter("rateFix", s =>
                {
                    s.Window = TimeSpan.FromSeconds(60);
                    s.PermitLimit = 1;
                    s.QueueLimit = 0;
                });
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

            builder.Services.AddScoped<IMinIOFood,MinIOFood>();

          //  builder.Services.Add

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




            //MinIO 

            builder.Services.AddSingleton<IMinioClient>(sp =>
            {
                return new MinioClient()
                    .WithEndpoint("localhost:9000")
                    .WithCredentials(builder.Configuration["Minio:AccessKey"], builder.Configuration["Minio:SecretKey"])
                    .WithSSL(false)
                    .Build();
            });



            //backgroundSerivce
            //builder.Services.AddHostedService<OutboxMessageProcessor>();

            builder.Services.AddControllers();

            var app = builder.Build();

            app.UseRateLimiter();

            app.UseMiddleware<CustomGlobalException>();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

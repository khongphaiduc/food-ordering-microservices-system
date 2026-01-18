using food_service.productservice.infastructure.ProductDbContexts;
using food_service.ProductService.API.Middlwares;
using food_service.ProductService.Application.Service;
using food_service.ProductService.Domain.Interface;
using food_service.ProductService.Infastructure.ImplementService;
using food_service.ProductService.Infastructure.RedisService.RedisInterface;
using food_service.ProductService.Infastructure.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

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


            //redis 

            builder.Services.AddTransient<IRedisLockService, RedisLockService>();

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration["RedisAddress"];
                options.InstanceName = "FoodTrungDuc";
            });

            var app = builder.Build();

            app.UseMiddleware<CustomGlobalException>();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

using food_service.productservice.infastructure.ProductDbContexts;
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


            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

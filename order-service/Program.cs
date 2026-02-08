using Microsoft.EntityFrameworkCore;
using order_service.OrderService.Infastructure.Models;

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

            builder.Services.AddControllers();

            var app = builder.Build();



            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

using Elastic.Clients.Elasticsearch;
using Microsoft.EntityFrameworkCore;
using search_service.Models;
using search_service.SearchService.Application.Interface;
using search_service.SearchService.Infastructure.ImplementServices;

namespace search_service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotNetEnv.Env.Load();

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<FoodProductsDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration["SQLPRODUCT"]);
            });



            builder.Services.AddScoped<IGetListProduct, GetListProduct>();
            builder.Services.AddScoped<ILoadFullProduct, LoadFullProduct>();



            builder.Services.AddControllers();


            // elasticsearch
            var settings = new ElasticsearchClientSettings(new Uri("http://localhost:9200")).DefaultIndex("products"); // nếu lúc truy vấn mà không khai báo index thì mặc định sẽ sử dụng Index : products

            var client = new ElasticsearchClient(settings);  // là object trung tâm để [Search , index , update, delete]

            builder.Services.AddSingleton(client);




            var app = builder.Build();



            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

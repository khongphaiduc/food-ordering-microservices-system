using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.EntityFrameworkCore;
using search_service.Models;
using search_service.SearchService.API.Middlware;
using search_service.SearchService.Application.Interface;
using search_service.SearchService.Infastructure.ConsumerRabbitMQ;
using search_service.SearchService.Infastructure.ImplementServices;
using search_service.SearchService.Infastructure.Redis.Interface;
using search_service.SearchService.Infastructure.Redis.Service;
using StackExchange.Redis;

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


            builder.Services.AddStackExchangeRedisCache(options =>          // AddStackExchangeRedisCache sẽ map IDistributeCatche
            {
                options.Configuration = builder.Configuration["HostRedis"];
                options.InstanceName = "ListProduct";
            });
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
                          ConnectionMultiplexer.Connect(builder.Configuration["HostRedis"]!));



            builder.Services.AddScoped<IRedisLockService, RedisLockService>();
            builder.Services.AddScoped<IGetListProduct, GetListProduct>();
            builder.Services.AddScoped<ILoadFullProduct, LoadFullProduct>();
            builder.Services.AddScoped<IElasticsearch, Elasticsearch>();



            builder.Services.AddHostedService<ElasticsearchConsumer>();

            // elasticsearch
            var settings = new ElasticsearchClientSettings(new Uri("https://localhost:9200"))
                 .DefaultIndex("products")
                 .Authentication(new BasicAuthentication("elastic", "tRKT9*XFLHZog*Wzmd2v"))
                 .ServerCertificateValidationCallback((sender, cert, chain, errors) => true);
            var client = new ElasticsearchClient(settings);  // là object trung tâm để [Search , index , update, delete]

            builder.Services.AddSingleton(client);



            builder.Services.AddControllers();
            var app = builder.Build();


            app.UseMiddleware<GlobalException>();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

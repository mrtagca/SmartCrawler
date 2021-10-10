using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using SmartCrawler.MongoDB.BaseTypes;
using SmartCrawler.MongoDB.DAL;
using SmartCrawler.MongoDbEntities;
using System;
using System.Xml;

namespace SmartCrawler.UrlFill
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
       Host.CreateDefaultBuilder(args)
           .ConfigureServices((hostContext, services) =>
           {
               var config = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json", optional: false)
                            .Build();

               string con = config.GetSection("MongoDbSettings").GetSection("ConnectionString").Value;

               services.Configure<MongoDbSettings>(options =>
               {
                   options.ConnectionString = config.GetSection("MongoDbSettings").GetSection("ConnectionString").Value;
                   options.Database = config.GetSection("MongoDbSettings").GetSection("Database").Value;
               });

               services.AddSingleton<IProductDal, ProductMongoDbDal>();
               services.AddHostedService<Worker>();
           });
    }
}

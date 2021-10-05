using DnsClient.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SmartCrawler.RabbitMQ.TestConsumerApplication.Workers;
using SmartCrawler.RabbitMQ.Types;
using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Hosting.WindowsServices;
using System.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SmartCrawler.MongoDB.BaseTypes;
using SmartCrawler.MongoDB.DAL;

namespace SmartCrawler.RabbitMQ.TestConsumerApplication
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
                   services.AddHostedService<TestWorker>();
               });
    }
}

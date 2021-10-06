using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartCrawler.MongoDB.BaseTypes;
using SmartCrawler.MongoDB.DAL;
using System;

namespace SmartCrawler.Distribute.ProductURLQueues
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

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SmartCrawler.MongoDB.DAL;
using SmartCrawler.MongoDbEntities;
using SmartCrawler.RabbitMQ;
using SmartCrawler.RabbitMQ.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCrawler.Distribute.ProductURLQueues
{
    public class Worker : BackgroundService
    {
        public static IConfigurationRoot Configuration { get; set; }
        private readonly IProductDal productDal;
        public List<RabbitMQBasicPublishModel> publishModels = new List<RabbitMQBasicPublishModel>();
        public List<Products> products;
        public List<string> stores;
        public Worker(IProductDal productDal)
        {
            var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            this.productDal = productDal;

            products = productDal.Get().ToList();
            stores = products.Select(x => x.StoreName).Distinct().ToList();

            foreach (string store in stores)
            {

                RabbitMQBasicPublishModel basicPublishModel = new RabbitMQBasicPublishModel();
                basicPublishModel.QueueConfiguration = new QueueConfiguration()
                {
                    HostName = Configuration.GetSection("WorkerSettings").GetSection("QueueHost").Value,
                    QueueName = store + "_ProductUrls_Queue",
                    Durable = false,
                    Exclusive = false,
                    AutoDelete = false,
                    Arguments = null
                };

                basicPublishModel.PublishConfiguration = new PublishConfiguration()
                {
                    Exchange = "",
                    RoutingKey = store + "_ProductUrls_Queue",
                    BasicProperties = null
                };

                RabbitMQPublisherComponent.Init(basicPublishModel);

                publishModels.Add(basicPublishModel);
            }
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = Environment.ProcessorCount;


            while (!stoppingToken.IsCancellationRequested)
            {
                int i = 0;

                foreach (RabbitMQBasicPublishModel pm in publishModels)
                {
                    foreach (var store in stores)
                    {
                        if (pm.QueueConfiguration.QueueName.Contains(store))
                        {
                            foreach (string pr in products.Where(x => x.StoreName == store).Select(x => x.ProductURL).Distinct().ToList())
                            {
                                pm.QueueMessage = pr;
                                RabbitMQPublisherComponent.BasicPublish(pm);
                                i++;

                            }
                            Console.WriteLine(i + " " + store + " URL eklendi => " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                        }
                    }

                }

                int waitTime = int.Parse(Configuration.GetSection("WorkerSettings").GetSection("ReadInterval").Value);
                Console.WriteLine("Worker " + waitTime / 60000 + " dakika bekletiliyor.");
                //Thread.Sleep(waitTime);
                await Task.Delay(waitTime,stoppingToken);
            }

        }
    }
}

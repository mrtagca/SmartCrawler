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
        private readonly IProductDal productDal;

        public List<RabbitMQBasicPublishModel> publishModels = new List<RabbitMQBasicPublishModel>();
        public List<Products> products;
        public List<string> stores;
        public Worker(IProductDal productDal)
        {
            this.productDal = productDal;

            products = productDal.Get().ToList();
            stores = products.Select(x => x.StoreName).Distinct().ToList();

            foreach (string store in stores)
            {

                RabbitMQBasicPublishModel basicPublishModel = new RabbitMQBasicPublishModel();
                basicPublishModel.QueueConfiguration = new QueueConfiguration()
                {
                    HostName = "localhost",
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
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                foreach (RabbitMQBasicPublishModel pm in publishModels)
                {
                    foreach (var store in stores)
                    {
                        if (pm.QueueConfiguration.QueueName.Contains(store))
                        {
                            foreach (string pr in products.Where(x=>x.StoreName == store).Select(x=>x.ProductURL).Distinct().ToList())
                            {
                                pm.QueueMessage = pr;
                                PublishBasic(pm);
                            }  
                        }
                    }
                }
                Console.WriteLine("Program 1 dakika bekletiliyor..");
                Thread.Sleep(5000);
            }
            
        }

        public static void PublishBasic(RabbitMQBasicPublishModel rabbitMQBasicPublishModel)
        {
            RabbitMQPublisherComponent.BasicPublish(rabbitMQBasicPublishModel);
        }


    }
}

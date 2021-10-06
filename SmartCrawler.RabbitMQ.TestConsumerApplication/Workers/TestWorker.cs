using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using RabbitMQ.Client.Events;
using SmartCrawler.MongoDB.DAL;
using SmartCrawler.MongoDbEntities;
using SmartCrawler.RabbitMQ.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCrawler.RabbitMQ.TestConsumerApplication.Workers
{
    public class TestWorker : BackgroundService
    {
        private readonly IProductDal productDal;

        public TestWorker(IProductDal productDal)
        {
            this.productDal = productDal;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            Products pr = new Products();
            pr.Id = ObjectId.GenerateNewId().ToString();
            pr.StoreName = "Teknosa";
            pr.Category = "Telefon";
            pr.SubCategory = "Cep Telefonu";
            pr.ThirdCategory = "Android Telefon";
            pr.Brand = "Samsung";
            pr.SKUName = "Samsung Galaxy Note20 Bronze Akıllı Telefon";
            pr.Price = 8499;
            pr.DiscountDescription = "%9 İndirim";
            pr.ProductURL = "https://www.teknosa.com/samsung-galaxy-note20-bronze-akilli-telefon-p-125077713";

            pr.ProductPhotos = new List<string>()
            {
                "https://reimg-teknosa-cloud-prod.mncdn.com/mnresize/600/600/productimage/125077713/125077713_0_MC/47005976.jpg",
"https://reimg-teknosa-cloud-prod.mncdn.com/mnresize/600/600/productimage/125077713/125077713_1_MC/47005979.jpg",
"https://reimg-teknosa-cloud-prod.mncdn.com/mnresize/600/600/productimage/125077713/125077713_2_MC/47005984.jpg",
"https://reimg-teknosa-cloud-prod.mncdn.com/mnresize/600/600/productimage/125077713/125077713_3_MC/47005988.jpg",
"https://reimg-teknosa-cloud-prod.mncdn.com/mnresize/600/600/productimage/125077713/125077713_4_MC/47005995.jpg",
"https://reimg-teknosa-cloud-prod.mncdn.com/mnresize/600/600/productimage/125077713/125077713_5_MC/47006017.jpg",
"https://reimg-teknosa-cloud-prod.mncdn.com/mnresize/600/600/productimage/125077713/125077713_6_MC/47006034.jpg",
"https://reimg-teknosa-cloud-prod.mncdn.com/mnresize/600/600/productimage/125077713/125077713_7_MC/47006044.jpg"
            };

            productDal.AddAsync(pr);

            RabbitMQBasicConsumeModel rabbitMQBasicConsumeModel = new RabbitMQBasicConsumeModel();
            rabbitMQBasicConsumeModel.QueueConfiguration = new QueueConfiguration()
            {
                HostName = "localhost",
                QueueName = "test4",
                Durable = false,
                Exclusive = false,
                AutoDelete = false,
                Arguments = null
            };

            rabbitMQBasicConsumeModel.BasicConsumeConfiguration = new BasicConsumeConfiguration()
            {
                Queue = "test4",
                AutoAck = true
            };

            RabbitMQConsumerComponent.Init(rabbitMQBasicConsumeModel);
            while (true)
            {
                RabbitMQConsumerComponent.BasicConsume(rabbitMQBasicConsumeModel, Consumer_Received);
            }


        }

        public static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(message);
        }
    }
}

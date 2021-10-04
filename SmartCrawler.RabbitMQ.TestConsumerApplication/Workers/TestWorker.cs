using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using RabbitMQ.Client.Events;
using SmartCrawler.RabbitMQ.TestConsumerApplication.Interface;
using SmartCrawler.RabbitMQ.TestConsumerApplication.Types;
using SmartCrawler.RabbitMQ.Types;
using System;
using System.Collections.Generic;
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
            pr.ProductName = "Test 2";

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

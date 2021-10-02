using RabbitMQ.Client;
using SmartCrawler.RabbitMQ.Types;
using System;
using System.Text;
using System.Threading;


namespace SmartCrawler.RabbitMQ.TestPublisherApplication
{
    class Program
    {
        static void Main(string[] args)
        {

            RabbitMQBasicPublishModel basicPublishModel = new RabbitMQBasicPublishModel();
            basicPublishModel.QueueConfiguration = new QueueConfiguration()
            {
                HostName = "localhost",
                QueueName = "test4",
                Durable = false,
                Exclusive = false,
                AutoDelete = false,
                Arguments = null
            };

            basicPublishModel.PublishConfiguration = new PublishConfiguration()
            {
                Exchange = "",
                RoutingKey = "test4",
                BasicProperties = null
            };

            RabbitMQPublisherComponent.Init(basicPublishModel);

            for (int i = 0; i < 1000000; i++)
            {
                //Thread.Sleep(200);
                
                basicPublishModel.QueueMessage = "Test : "+i;
                RabbitMQPublisherComponent.BasicPublish(basicPublishModel); 
            }

        }
    }
}

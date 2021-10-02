using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SmartCrawler.RabbitMQ.Types;
using System;
using System.Text;

namespace SmartCrawler.RabbitMQ.TestConsumerApplication
{
    class Program
    {
        static void Main(string[] args)
        {
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

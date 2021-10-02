using RabbitMQ.Client;
using SmartCrawler.RabbitMQ.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCrawler.RabbitMQ
{
    public class RabbitMQPublisherComponent
    {
        public static ConnectionFactory factory;
        public static IConnection connection;
        public static IModel channel;

        public static void Init(RabbitMQBasicPublishModel basicPublishModel)
        {
            factory = new ConnectionFactory() { HostName = basicPublishModel.QueueConfiguration.HostName };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
        }
        public static void BasicPublish(RabbitMQBasicPublishModel basicPublishModel)
        {
            //var factory = new ConnectionFactory() { HostName = basicPublishModel.QueueConfiguration.HostName };
            //using (var connection = factory.CreateConnection())
            //using (var channel = connection.CreateModel())
            //{
                channel.QueueDeclare(
                        queue: basicPublishModel.QueueConfiguration.QueueName,
                        durable: basicPublishModel.QueueConfiguration.Durable, //in memory : false -- persistence true
                        exclusive: basicPublishModel.QueueConfiguration.Exclusive, //kuyruk başka connectionlar ile birlikte kullanılacaksa true, kullanılmayacaksa false
                        autoDelete: basicPublishModel.QueueConfiguration.AutoDelete, // Kuyruğa gönderilen veri consumer tarafına geçtiğinde silinecekse true, silinmeyecekse false
                        arguments: basicPublishModel.QueueConfiguration.Arguments
                        );

                string message = basicPublishModel.QueueMessage;

                var body = Encoding.UTF8.GetBytes(basicPublishModel.QueueMessage);

                channel.BasicPublish(
                    exchange: basicPublishModel.PublishConfiguration.Exchange,
                    routingKey: basicPublishModel.PublishConfiguration.RoutingKey,
                    basicProperties: basicPublishModel.PublishConfiguration.BasicProperties,
                    body: body
                    );
            //}
        }
    }
}

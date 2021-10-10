using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SmartCrawler.RabbitMQ.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCrawler.RabbitMQ
{
    public class RabbitMQConsumerComponent
    {
        public static ConnectionFactory factory;
        public static IConnection connection;
        public static IModel channel;

        public static void Init(RabbitMQBasicConsumeModel rabbitMQBasicConsumeModel)
        {
            factory = new ConnectionFactory() { HostName = rabbitMQBasicConsumeModel.QueueConfiguration.HostName };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
        }
        public static void BasicConsume(RabbitMQBasicConsumeModel rabbitMQBasicConsumeModel, EventHandler<BasicDeliverEventArgs> receivedHandler)
        {
            //var factory = new ConnectionFactory() { HostName = rabbitMQBasicConsumeModel.QueueConfiguration.HostName };
            //using (var connection = factory.CreateConnection())
            //using (var channel = connection.CreateModel())
            //{
                channel.QueueDeclare(
               queue: rabbitMQBasicConsumeModel.QueueConfiguration.QueueName,
               durable: rabbitMQBasicConsumeModel.QueueConfiguration.Durable,
               exclusive: rabbitMQBasicConsumeModel.QueueConfiguration.Exclusive,
               autoDelete: rabbitMQBasicConsumeModel.QueueConfiguration.AutoDelete,
               arguments: rabbitMQBasicConsumeModel.QueueConfiguration.Arguments
               );

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += receivedHandler;

            channel.BasicConsume(
                queue: rabbitMQBasicConsumeModel.QueueConfiguration.QueueName,
                autoAck: rabbitMQBasicConsumeModel.BasicConsumeConfiguration.AutoAck,
                consumer: consumer
                );
        //};
    }

        public static string BasicGet(RabbitMQBasicConsumeModel rabbitMQBasicConsumeModel)
        {
            //var factory = new ConnectionFactory() { HostName = rabbitMQBasicConsumeModel.QueueConfiguration.HostName };
            //using (var connection = factory.CreateConnection())
            //using (var channel = connection.CreateModel())
            //{
            channel.QueueDeclare(
               queue: rabbitMQBasicConsumeModel.QueueConfiguration.QueueName,
               durable: rabbitMQBasicConsumeModel.QueueConfiguration.Durable,
               exclusive: rabbitMQBasicConsumeModel.QueueConfiguration.Exclusive,
               autoDelete: rabbitMQBasicConsumeModel.QueueConfiguration.AutoDelete,
               arguments: rabbitMQBasicConsumeModel.QueueConfiguration.Arguments
               );

            var consumer = new EventingBasicConsumer(channel);

            var data = channel.BasicGet(rabbitMQBasicConsumeModel.QueueConfiguration.QueueName, true);
            var message = System.Text.Encoding.UTF8.GetString(data.Body.ToArray());

            return message;
            //};
        }
    }
}

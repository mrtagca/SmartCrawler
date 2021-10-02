using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCrawler.RabbitMQ.Types
{
    public class PublishConfiguration
    {
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }
        public IBasicProperties BasicProperties { get; set; }

    }
}

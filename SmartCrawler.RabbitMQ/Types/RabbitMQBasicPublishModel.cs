using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCrawler.RabbitMQ.Types
{
    public class RabbitMQBasicPublishModel
    {
        public QueueConfiguration QueueConfiguration { get; set; }
        public PublishConfiguration PublishConfiguration { get; set; }
        public string QueueMessage { get; set; }
    }


}

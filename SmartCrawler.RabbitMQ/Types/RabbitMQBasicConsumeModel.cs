using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCrawler.RabbitMQ.Types
{
    public class RabbitMQBasicConsumeModel
    {
        public QueueConfiguration QueueConfiguration { get; set; }
        public BasicConsumeConfiguration BasicConsumeConfiguration { get; set; }
    }
}

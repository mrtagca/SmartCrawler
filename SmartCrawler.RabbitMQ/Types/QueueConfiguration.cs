using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCrawler.RabbitMQ.Types
{
    public class QueueConfiguration
    {
        public string HostName { get; set; }
        public string QueueName { get; set; }
        public bool Durable { get; set; }
        public bool Exclusive { get; set; }
        public bool AutoDelete { get; set; }
        public IDictionary<string, object> Arguments { get; set; }
    }
}

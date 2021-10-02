using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCrawler.RabbitMQ.Types
{
    public class BasicConsumeConfiguration
    {
        public string Queue { get; set; }
        public bool AutoAck { get; set; }

    }
}

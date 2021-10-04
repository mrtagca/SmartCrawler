using SmartCrawler.DbAccess.MongoDB.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCrawler.RabbitMQ.TestConsumerApplication.Types
{
    public class Products : MongoDbEntity
    {
        public string ProductName { get; set; }
    }
}

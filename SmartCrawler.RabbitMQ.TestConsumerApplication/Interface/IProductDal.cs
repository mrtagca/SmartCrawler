using SmartCrawler.DbAccess.MongoDB.Repositories;
using SmartCrawler.RabbitMQ.TestConsumerApplication.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCrawler.RabbitMQ.TestConsumerApplication.Interface
{
    public interface IProductDal : IRepository<Products, string>
    {
    }
}

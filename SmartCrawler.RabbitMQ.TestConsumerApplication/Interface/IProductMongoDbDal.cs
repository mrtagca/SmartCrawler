using Microsoft.Extensions.Options;
using SmartCrawler.DbAccess.MongoDB.Configuration;
using SmartCrawler.DbAccess.MongoDB.Repositories;
using SmartCrawler.RabbitMQ.TestConsumerApplication.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCrawler.RabbitMQ.TestConsumerApplication.Interface
{
    public class ProductMongoDbDal : MongoDbRepositoryBase<Products>, IProductDal
    {
        public ProductMongoDbDal(IOptions<MongoDbSettings> options) : base(options)
        {
        }
    }
}

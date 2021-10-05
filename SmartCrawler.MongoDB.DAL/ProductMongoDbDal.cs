using Microsoft.Extensions.Options;
using SmartCrawler.MongoDB.BaseTypes;
using SmartCrawler.MongoDB.Repositories;
using SmartCrawler.MongoDbEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCrawler.MongoDB.DAL
{
    public class ProductMongoDbDal : MongoDbRepositoryBase<Products>, IProductDal
    {
        public ProductMongoDbDal(IOptions<MongoDbSettings> options) : base(options)
        {
        }
    }
}

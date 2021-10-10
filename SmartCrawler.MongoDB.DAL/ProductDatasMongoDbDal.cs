using Microsoft.Extensions.Options;
using SmartCrawler.MongoDB.BaseTypes;
using SmartCrawler.MongoDB.Repositories;
using SmartCrawler.MongoDbEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCrawler.MongoDB.DAL
{
    public class ProductDatasMongoDbDal : MongoDbRepositoryBase<ProductDatas>, IProductDatasDal
    {
        public ProductDatasMongoDbDal(IOptions<MongoDbSettings> options) : base(options)
        {
        }
    }
}

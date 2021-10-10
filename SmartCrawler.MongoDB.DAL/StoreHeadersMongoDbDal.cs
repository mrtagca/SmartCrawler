using Microsoft.Extensions.Options;
using SmartCrawler.MongoDB.BaseTypes;
using SmartCrawler.MongoDB.Repositories;
using SmartCrawler.MongoDbEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCrawler.MongoDB.DAL
{
    public class StoreHeadersMongoDbDal : MongoDbRepositoryBase<StoreHeaders>, IStoreHeadersDal
    {
        public StoreHeadersMongoDbDal(IOptions<MongoDbSettings> options) : base(options)
        {
        }
    }
}

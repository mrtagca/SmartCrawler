using SmartCrawler.MongoDB.Repositories;
using SmartCrawler.MongoDbEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCrawler.MongoDB.DAL
{
    public interface IStoreHeadersDal : IRepository<StoreHeaders, string>
    {
    }
}

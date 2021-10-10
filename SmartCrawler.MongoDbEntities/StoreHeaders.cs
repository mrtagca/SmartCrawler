using SmartCrawler.MongoDB.BaseTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCrawler.MongoDbEntities
{
    public class StoreHeaders : MongoDbEntity
    {
        public string StoreName { get; set; }
        public string HeaderKey { get; set; }
        public string HeaderValue { get; set; }
    }
}

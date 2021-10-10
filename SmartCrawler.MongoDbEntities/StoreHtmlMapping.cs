using SmartCrawler.MongoDB.BaseTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCrawler.MongoDbEntities
{
    public class StoreHtmlMapping : MongoDbEntity
    {
        public string StoreName { get; set; }
        public string ColumnHeader { get; set; }
        public string XpathPattern { get; set; }
        public bool IsAttributeCrawl { get; set; }
        public string AttributeName { get; set; }
    }
}

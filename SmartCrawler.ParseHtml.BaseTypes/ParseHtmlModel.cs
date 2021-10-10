using SmartCrawler.MongoDbEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCrawler.ParseHtml.BaseTypes
{
    public class ParseHtmlModel
    {
        public string StoreName { get; set; }
        public string URL { get; set; }
        public string HtmlString { get; set; }
        public List<StoreHtmlMapping> StoreHtmlMappings { get; set; }

    }
}

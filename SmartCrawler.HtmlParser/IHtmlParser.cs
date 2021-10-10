using SmartCrawler.MongoDbEntities;
using SmartCrawler.ParseHtml.BaseTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCrawler.HtmlParser
{
    public interface IHtmlParser
    {
        public List<ProductDatas> ParseHtml(ParseHtmlModel parseHtmlModel);
    }
}

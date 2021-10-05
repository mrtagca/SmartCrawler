using SmartCrawler.MongoDB.BaseTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCrawler.MongoDbEntities
{
    public class Products : MongoDbEntity
    {
        public string StoreName { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string ThirdCategory { get; set; }
        public string Brand { get; set; }
        public string SKUName { get; set; }
        public double Price { get; set; }
        public string DiscountDescription { get; set; }
        public string ProductURL { get; set; }
        public List<string> ProductPhotos { get; set; }
    }
}

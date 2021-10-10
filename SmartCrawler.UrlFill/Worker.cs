using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using SmartCrawler.MongoDB.DAL;
using SmartCrawler.MongoDbEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace SmartCrawler.UrlFill
{
    public class Worker : BackgroundService
    {
        public IProductDal productDal { get; set; }
        public Worker(IProductDal productDal)
        {
            this.productDal = productDal;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            bool IsOk = true;

            while (IsOk)
            {
                string xmlDosyasi = @"C:\Users\admin\Desktop\teknosa-products.xml";
                XmlTextReader XmlOkuyucu = new XmlTextReader(xmlDosyasi);

                List<Products> productList = new List<Products>();

                while (XmlOkuyucu.Read())
                {
                    if (XmlOkuyucu.NodeType == XmlNodeType.Element)
                    {
                        if (XmlOkuyucu.Name == "loc")
                        {
                            Products products = new Products();
                            products.Id = ObjectId.GenerateNewId().ToString();
                            products.StoreName = "Teknosa";
                            products.ProductURL = XmlOkuyucu.ReadElementContentAsString();

                            productDal.Add(products);
                        }

                    }
                }

                XmlOkuyucu.Close();
                IsOk = false;
            }

            
        }
    }
}

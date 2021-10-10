using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RestSharp;
using SmartCrawler.HtmlParser.Repositories;
using SmartCrawler.MongoDB.DAL;
using SmartCrawler.MongoDbEntities;
using SmartCrawler.ParseHtml.BaseTypes;
using SmartCrawler.RabbitMQ;
using SmartCrawler.RabbitMQ.Types;
using SmartCrawler.Requestor.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCrawler.QueueUrlConsumer.MediaMarkt
{
    public class Worker : BackgroundService
    {

        public static IConfigurationRoot Configuration { get; set; }
        public IStoreHeadersDal storeHeadersDal;
        public IStoreHtmlMappingDal storeHtmlMappingDal;
        public static IProductDatasDal productDatasDal { get; set; }
        public static List<StoreHeaders> storeHeaders;
        public static List<StoreHtmlMapping> storeHtmlMappings;

        public Worker(IStoreHeadersDal storeHeadersDal, IStoreHtmlMappingDal storeHtmlMappingDal, IProductDatasDal productDatas)
        {
            var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            this.storeHeadersDal = storeHeadersDal;
            this.storeHtmlMappingDal = storeHtmlMappingDal;
            productDatasDal = productDatas;
            storeHeaders = storeHeadersDal.Get(x => x.StoreName == Configuration.GetSection("WorkerSettings").GetSection("StoreName").Value).ToList();
            storeHtmlMappings = storeHtmlMappingDal.Get(x => x.StoreName == Configuration.GetSection("WorkerSettings").GetSection("StoreName").Value).ToList();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            RabbitMQBasicConsumeModel rabbitMQBasicConsumeModel = new RabbitMQBasicConsumeModel();

            rabbitMQBasicConsumeModel.QueueConfiguration = new QueueConfiguration()
            {
                HostName = Configuration.GetSection("WorkerSettings").GetSection("QueueHost").Value,
                QueueName = Configuration.GetSection("WorkerSettings").GetSection("StoreName").Value + "_ProductUrls_Queue",
                Durable = false,
                Exclusive = false,
                AutoDelete = false,
                Arguments = null
            };

            rabbitMQBasicConsumeModel.BasicConsumeConfiguration = new BasicConsumeConfiguration()
            {
                Queue = Configuration.GetSection("WorkerSettings").GetSection("StoreName").Value + "_ProductUrls_Queue",
                AutoAck = true
            };

            RabbitMQConsumerComponent.Init(rabbitMQBasicConsumeModel);

            while (true)
            {
                RabbitMQConsumerComponent.BasicConsume(rabbitMQBasicConsumeModel, Consumer_Received);
            }
        }

        public static int i = 0;
        public static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                var body = e.Body.ToArray();
                var url = Encoding.UTF8.GetString(body);

                RequestModel requestModel = new RequestModel();
                requestModel.EndPoint = url;
                requestModel.Method = Method.GET;


                Dictionary<string, string> headers = new Dictionary<string, string>();
                foreach (var item in storeHeaders)
                {
                    headers.Add(item.HeaderKey, item.HeaderValue);
                }
                requestModel.Headers = headers;

                IRestResponse response = Requestor.Requestor.Request(requestModel);
                string html = response.Content;

                if (!string.IsNullOrWhiteSpace(html))
                {
                    ParseHtmlModel parseHtmlModel = new ParseHtmlModel();
                    parseHtmlModel.StoreName = Configuration.GetSection("WorkerSettings").GetSection("StoreName").Value;
                    parseHtmlModel.URL = url;
                    parseHtmlModel.HtmlString = html;
                    parseHtmlModel.StoreHtmlMappings = storeHtmlMappings;

                    List<ProductDatas> productDatas = new HtmlParserRepositoryBase().ParseHtml(parseHtmlModel);
                    productDatas.AddRange(productDatas);

                    foreach (ProductDatas item in productDatas)
                    {
                        productDatasDal.Add(item);
                    }
                }

                Console.WriteLine(i + ". link eklendi.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Link eklenemedi. Error: " + ex.Message + " InnerException:" + ex.InnerException + " StackTrace:" + ex.StackTrace);
            }
        }
    }
}

using RestSharp;
using SmartCrawler.Requestor.BaseTypes;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SmartCrawler.Requestor
{
    public static class Requestor
    {
        public static IRestResponse Request(RequestModel requestModel)
        {
            var client = new RestClient(requestModel.EndPoint);
            client.Timeout = -1;
            var request = new RestRequest(requestModel.Method);


            foreach (var item in requestModel.Headers)
            {
                request.AddHeader(item.Key, item.Value);
            }

            IRestResponse response = client.Execute(request);
            return response;
        }
    }
}

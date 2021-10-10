using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SmartCrawler.Requestor.BaseTypes
{
    public class RequestModel
    {
        public string EndPoint { get; set; }
        public Method Method { get; set; }
        public Dictionary<string,string> Headers { get; set; }
    }
}

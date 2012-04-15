using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using Raven.Client.Document;
using ScrapporNet.Extensions;

namespace ScrapporNet
{
    static class Program
    {
        static void Main(string[] args)
        {
            //var s = new Stopwatch();
            //Parse.ParseWinesFromSearchResults();
            //Fetch.FetchWinesDetailsPages();
            Parse
                .ParseWineDetailPages();
            //Fetch.DownloadWinePages();
            Console.ReadLine();
        }
    }

    class Configuration
    {
        private String _server { get; set; }
        private int _timeout { get; set; }
        private int _port { get; set; }

        public Configuration SetPort(int port)
        {
            _port = port;
            return this;
        }

        public Configuration SetServer(String server)
        {
            _server = server;
            return this;
        }

        public Configuration SetTimeout(int timeout)
        {
            _timeout = timeout;
            return this;
        }
    }

}

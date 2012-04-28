using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using Ninject;
using Ninject.Modules;
using Raven.Client;
using Raven.Client.Document;
using ScrapporNet.Extensions;

namespace ScrapporNet
{
    static class Program
    {
        static void Main(string[] args)
        {
            //var s = new Stopwatch();
            //Fetch.FetchWinesDetailsPages();
            //Parse
                //.ParseWineDetailPages()
                //.ParseWinesFromSearchResults();

            new Parse(new DocumentStore { ConnectionStringName = "CS" }.Initialize()).ParseWinesFromSearchResults();
            //Fetch.DownloadWinePages();
            Console.ReadLine();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using Raven.Client.Document;

namespace ScrapporNet
{
    class Program
    {
        static void Main(string[] args)
        {
            //var s = new Stopwatch();
            //Fetch.ParseWinesFromSearchResults();
            Fetch.FetchWinesDetailsPages();
            //Fetch.DownloadWinePages();
            Console.ReadLine();
        }
    }
}

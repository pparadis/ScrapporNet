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
            var s = new Stopwatch();
            var path = SetupFolders();

            var f = new Fetch(path);
            //f.DownloadWineListPages();

            var p = new Parse(path);
            p.ParseWinesFromSearchResults();
            f.FetchWinesDetailsPages();
            //p.ParseWineDetailPages();

            Console.ReadLine();
        }

        private static string SetupFolders()
        {
            var dateTimeTimeStamp = "201601011502560735" /*DateTime.Now.GetTimestamp()*/;
            //var dateTimeTimeStamp = DateTime.Now.GetTimestamp();
            //var dateTimeTimeStamp = DateTime.Now.GetTimestamp();
            System.IO.Directory.CreateDirectory(@"d:\Wine\" + dateTimeTimeStamp + @"\details\");
            return (@"d:\Wine\" + dateTimeTimeStamp + @"\");
        }
    }
}

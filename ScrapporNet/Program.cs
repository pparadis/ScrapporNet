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

            f.DownloadWineListPages();

            

            var p = new Parse(new DocumentStore
                    {
                        ConnectionStringName = "CS"
                    }.Initialize(), path);

            p.ParseWinesFromSearchResults();

            f.FetchWinesDetailsPages();

            Console.ReadLine();
        }

        private static string SetupFolders()
        {
            var dateTimeTimeStamp = "201210272324114400" /*DateTime.Now.GetTimestamp()*/;
            System.IO.Directory.CreateDirectory(@"e:\Wine\" + dateTimeTimeStamp + @"\details\");
            //return (@"e:\Wine\" + dateTimeTimeStamp + @"\");
            return (@"e:\Wine\" + dateTimeTimeStamp + @"\");
        }
    }
}

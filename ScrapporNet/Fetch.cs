﻿using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Raven.Client.Document;
using ScrapporNet.Entities;
using ScrapporNet.Extensions;

namespace ScrapporNet
{
    public class Fetch
    {
        private const int PAGE_SIZE = 1024;
        private string FilePath { get; set; }

        public Fetch(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                SetupFolders();
            } 
            else
            {
                FilePath = filePath;
            }
        }

        public Fetch()
        {
            SetupFolders();
        }

        private void SetupFolders()
        {
            var dateTimeTimeStamp = DateTime.Now.GetTimestamp();
            System.IO.Directory.CreateDirectory(@"e:\Wine\" + dateTimeTimeStamp);
            FilePath = (@"e:\Wine\" + dateTimeTimeStamp + @"\");
        }

        public void FetchWinesDetailsPages()
        {
            var documentStore = new DocumentStore
            {
                ConnectionStringName = "CS"
            }.Initialize();

            using (var session = documentStore.OpenSession())
            {
                var wineList = session.Query<Wine>();
                var winePageCount = (wineList.Count().RoundOff() / PAGE_SIZE);

                for (var i = 0; i <= winePageCount; i++)
                {
                    Console.WriteLine("Fetching page " + i);
                    var results = session
                        .Query<Wine>()
                        .Skip(i * PAGE_SIZE)
                        .Take(PAGE_SIZE)
                        .ToList();

                    for (int j = 0; j < results.Count(); j++)
                    {
                        Console.WriteLine("Page : " + i + " - Element # " + j + " - " + results[j]);
                        Thread.Sleep(500);
                        DownloadWinePages(results[j]);
                    }

                    Console.WriteLine("Fetch of page " + i + " done.");
                    Thread.Sleep(2000);
                }

                Console.WriteLine("Downloaded " + wineList.Count() + " wine pages.");
            }
        }

        private void DownloadWinePages(Wine wine)
        {
            var web = new WebClient { Encoding = Encoding.UTF8 };
            web.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            var regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            var wineFileName = r.Replace(wine.Name.Replace(" ", "_"), "");

            var file = web.DownloadString(wine.Url);
            File.WriteAllText(FilePath  + @"details\" + wineFileName + "_" + wine.Id + " .html", file, Encoding.UTF8);
        }

        public void DownloadWineListPages()
        {
            var web = new WebClient { Encoding = Encoding.UTF8 };
            web.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            for (var i = 0; i <= 9133; i = i + 100)
            {
                Thread.Sleep(500);
                Console.WriteLine(i);

                var req = @"http://www.saq.com/webapp/wcs/stores/servlet/CatalogSearchResultView?storeId=10001&langId=-2&catalogId=10001&searchTerm=&resultCatEntryType=&beginIndex=" + i + "&tri=RechercheUCIProdDescAttributeInfo&sensTri=AscOperator&searchType=400&codeReseau=&categoryId=11748&viewTaskName=SAQCatalogSearchResultView&catalogVenteId=&pageSize=100";
                File.WriteAllText(FilePath + "saq_" + i + ".html", web.DownloadString(req), Encoding.UTF8);
            }
        }
    }
}

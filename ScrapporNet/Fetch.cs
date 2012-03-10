using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using HtmlAgilityPack;
using Raven.Client.Document;

namespace ScrapporNet
{
    static class Fetch
    {
        public static void ParseWinesFromSearchResults()
        {
            var documentStore = new DocumentStore
            {
                ConnectionStringName = "CS"
            }.Initialize();

            using (var session = documentStore.OpenSession())
            {
                var doc = new HtmlDocument();
                var files = Directory.GetFiles(@"e:\wine\", "*.html").OrderBy(p => p.ToString(), new NaturalStringComparer());
                foreach (var file in files)
                {
                    doc.Load(file, Encoding.UTF8);
                    var wines = doc.DocumentNode.SelectNodes("//table[@class='recherche']/tbody/tr[*]/td[2]");
                    foreach (var info in wines)
                    {
                        var wineName = info.ChildNodes[1].InnerHtml.Trim();
                        var wineUrl = "http://www.saq.com/webapp/wcs/stores/servlet/" + info.ChildNodes[1].Attributes["href"].Value;

                        var wineDesc = info.ChildNodes[3].InnerHtml.Replace("\t", "").Replace("\n", "").Replace("\r", "").Replace("&nbsp;", " ").Trim();

                        var wineProperties = wineDesc.Split(',').ToList();

                        if (wineProperties[2].Trim() == "00002008")
                        {
                            continue;
                        }

                        var entity = new Wine
                                            {
                                                Name = HttpUtility.HtmlDecode(wineName),
                                                Url = wineUrl,
                                                Color = wineProperties[0] ?? "",
                                                Nature = wineProperties[1] ?? "",
                                                Format = wineProperties[2] ?? "",
                                                Id = wineProperties[3] ?? ""

                                            };

                        session.Store(entity);
                    }
                }
                session.SaveChanges();
            }
        }

        public static void ParseWinesDetailsPages()
        {

        }

        public static void FetchWinesDetailsPages()
        {
            var documentStore = new DocumentStore
            {
                ConnectionStringName = "CS"
            }.Initialize();

            using (var session = documentStore.OpenSession())
            {
                var wineList = session.Query<Wine>();

                var winePageCount = (wineList.Count()/10).RoundOff();

                for (var i = 0; i <= winePageCount; i++)
                {
                    Console.WriteLine("Fetching page " + i);
                    var results = session
                        .Query<Wine>()
                        .Skip(i*10)
                        .Take(10)
                        .ToList();

                    foreach (var wine in results)
                    {
                        Console.WriteLine(wine + " - " + wine.Url);
                        Thread.Sleep(500);
                        DownloadWinePages(wine);
                    }
                    Console.WriteLine("Fetch of page " + i + " done.");
                    Thread.Sleep(2000);
                }
                
                Console.WriteLine("Downloaded " + wineList.Count() + " wine pages.");
            }
        }

        private static void DownloadWinePages(Wine wine)
        {
            var web = new WebClient { Encoding = Encoding.UTF8 };
            web.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            var regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            var wineFileName = r.Replace(wine.Name.Replace(" ", "_"), "");

            File.WriteAllText(@"e:\wine\details\" + wineFileName + " .html", web.DownloadString(wine.Url), Encoding.UTF8);
        }

        public static void DownloadWineListPages()
        {
            var web = new WebClient { Encoding = Encoding.UTF8 };
            web.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            for (var i = 0; i <= 10830; i = i + 100)
            {
                Thread.Sleep(500);
                Console.WriteLine(i);

                var req = @"http://www.saq.com/webapp/wcs/stores/servlet/CatalogSearchResultView?storeId=10001&langId=-2&catalogId=10001&searchTerm=&resultCatEntryType=&beginIndex=" + i + "&tri=RechercheUCIProdDescAttributeInfo&sensTri=AscOperator&searchType=100&codeReseau=&categoryId=&viewTaskName=SAQCatalogSearchResultView&catalogVenteId=&pageSize=100";
                File.WriteAllText(@"e:\wine\saq_" + i + ".html", web.DownloadString(req), Encoding.UTF8);
            }
        }
    }

    [SuppressUnmanagedCodeSecurity]
    internal static class SafeNativeMethods
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        public static extern int StrCmpLogicalW(string psz1, string psz2);
    }

    public sealed class NaturalStringComparer : IComparer<string>
    {
        public int Compare(string a, string b)
        {
            return SafeNativeMethods.StrCmpLogicalW(a, b);
        }
    }

    public sealed class NaturalFileInfoNameComparer : IComparer<FileInfo>
    {
        public int Compare(FileInfo a, FileInfo b)
        {
            return SafeNativeMethods.StrCmpLogicalW(a.Name, b.Name);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Web;
using HtmlAgilityPack;
using Raven.Client.Document;

namespace ScrapporNet
{
    static class Fetch
    {
        public static void FetchWinePages()
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
                    Console.WriteLine("File - " + file);
                    doc.Load(file, Encoding.UTF8);
                    var wines = doc.DocumentNode.SelectNodes("//table[@class='recherche']/tbody/tr[*]/td[2]");
                    foreach (var info in wines)
                    {
                        var wineName = info.ChildNodes[1].InnerHtml.Trim();
                        var wineDesc = info.ChildNodes[3].InnerHtml.Replace("\t", "").Replace("\n", "").Replace("\r", "").Replace("&nbsp;", " ").Trim();

                        var wineProperties = wineDesc.Split(',').ToList();

                        if (wineProperties[2].Trim() == "00002008")
                        {
                            continue;
                        }

                        var entity = new Wine
                                            {
                                                Name = HttpUtility.HtmlDecode(wineName),
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

        //Console.WriteLine(HttpUtility.HtmlDecode(wineName));
        //Console.WriteLine("Couleur : " + wineDesc.Split(',')[0]);
        //Console.WriteLine("Nature : " + wineDesc.Split(',')[1]);
        //Console.WriteLine("Format : " + wineDesc.Split(',')[2]);
        //Console.WriteLine("Code SAQ : " + wineDesc.Split(',')[3]);
        //Console.WriteLine("---------------------------------");


        public static void FetchWinePages2()
        {
            var doc = new HtmlDocument();
            doc.Load(@"E:\wine\saq_500.html", Encoding.UTF8);
            var winesNames = doc.DocumentNode.SelectNodes("//table[@class='recherche']/tbody/tr[*]/td[2]/a");
            var wineDesc = doc.DocumentNode.SelectNodes("//table[@class='recherche']/tbody/tr[*]/td[2]/text()[2]");

            //Console.WriteLine("Wines names : " + winesNames.Count);
            //Console.WriteLine("Wines WineDesc : " + wineDesc.Count);

            if (wineDesc.Count != winesNames.Count)
            {
                return;
            }

            for (var i = 0; i < winesNames.Count; i++)
            {
                var wineName = winesNames[i].InnerHtml.Trim();
                var wineDescContent = wineDesc[i].InnerHtml.Replace("\t", "").Replace("\n", "").Replace("\r", "").Replace("&nbsp;", " ").Trim();

                //Console.WriteLine(HttpUtility.HtmlDecode(wineName));
                //Console.WriteLine("Couleur : " + wineDescContent.Split(',')[0]);
                //Console.WriteLine("Nature : " + wineDescContent.Split(',')[1]);
                //Console.WriteLine("Format : " + wineDescContent.Split(',')[2]);
                //Console.WriteLine("Code SAQ : " + wineDescContent.Split(',')[3]);
                //Console.WriteLine("---------------------------------");
            }
        }

        public static void DownloadWinePages()
        {
            var web = new WebClient { Encoding = Encoding.UTF8 };
            web.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            for (var i = 0; i <= 10830; i = i + 100)
            {
                Thread.Sleep(500);
                Console.WriteLine(i);

                var req = @"http://www.saq.com/webapp/wcs/stores/servlet/CatalogSearchResultView?storeId=10001&langId=-2&catalogId=10001&searchTerm=&resultCatEntryType=&beginIndex=" + i + "&tri=RechercheUCIProdDescAttributeInfo&sensTri=AscOperator&searchType=100&codeReseau=&categoryId=&viewTaskName=SAQCatalogSearchResultView&catalogVenteId=&pageSize=100";
                try
                {
                    System.IO.File.WriteAllText(@"e:\wine\saq_" + i + ".html", web.DownloadString(req), Encoding.UTF8);
                }
                catch (WebException we)
                {
                    throw;
                }

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using HtmlAgilityPack;

namespace ScrapporNet
{
    class Fetch
    {
        public static void FetchWinePages()
        {
            var doc = new HtmlDocument();
            doc.Load(@"E:\wine\saq_500.html", Encoding.UTF8);
            var wines = doc.DocumentNode.SelectNodes("//table[@class='recherche']/tbody/tr[*]/td[2]");
            foreach (var info in wines)
            {
                var wineName = info.ChildNodes[1].InnerHtml.Trim();
                var wineDesc = info.ChildNodes[3].InnerHtml.Replace("\t", "").Replace("\n", "").Replace("\r", "").Replace("&nbsp;", " ").Trim();

                Console.WriteLine(HttpUtility.HtmlDecode(wineName));
                Console.WriteLine("Couleur : " + wineDesc.Split(',')[0]);
                Console.WriteLine("Nature : " + wineDesc.Split(',')[1]);
                Console.WriteLine("Format : " + wineDesc.Split(',')[2]);
                Console.WriteLine("Code SAQ : " + wineDesc.Split(',')[3]);
                Console.WriteLine("---------------------------------");
            }
        }

        public static void DownloadWinePages()
        {
            var web = new WebClient { Encoding = Encoding.UTF8 };
            web.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            for (var i = 0; i < 1000; i = i + 100)
            {
                var req = @"http://www.saq.com/webapp/wcs/stores/servlet/CatalogSearchResultView?storeId=10001&langId=-2&catalogId=10001&searchTerm=&resultCatEntryType=&beginIndex=" + i + "&tri=RechercheUCIProdDescAttributeInfo&sensTri=AscOperator&searchType=100&codeReseau=&categoryId=&viewTaskName=SAQCatalogSearchResultView&catalogVenteId=&pageSize=100";
                System.IO.File.WriteAllText(@"e:\wine\saq_" + i + ".html", web.DownloadString(req), Encoding.UTF8);
            }
        }
    }
}

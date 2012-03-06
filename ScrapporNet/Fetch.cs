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
            doc.Load(@"E:\wine\saq_200.html", Encoding.UTF8);
            var wines = doc.DocumentNode.SelectNodes("//table[@class='recherche']/tbody/tr[*]");
            foreach (var wineRow in wines)
            {
                var infos = wineRow.SelectNodes("td[2]");
                foreach (var info in infos)
                {
                    var wineName = info.ChildNodes[1].InnerHtml.Trim(); //Name
                    var wineDesc = info.ChildNodes[3].InnerHtml.Replace("\t", "").Replace("\n", "").Replace("\r","").Replace("&nbsp;", " ").Trim(); //Name

                    Console.WriteLine(HttpUtility.HtmlDecode(wineName));
                    //Console.WriteLine("---");
                    Console.WriteLine("Couleur : " + wineDesc.Split(',')[0]);
                    Console.WriteLine("Nature : " + wineDesc.Split(',')[1]);
                    Console.WriteLine("Format : " + wineDesc.Split(',')[2]);
                    Console.WriteLine("Code SAQ : " + wineDesc.Split(',')[3]);
                    //Console.WriteLine(HttpUtility.HtmlDecode(wineDesc));
                    Console.WriteLine("---------------------------------");
                }
            }

            
            
            //doc.Load("http://www.saq.com/webapp/wcs/stores/servlet/CatalogSearchResultView?storeId=10001&catalogId=10001&resultCatEntryType=2&beginIndex=0&tri=RechercheUCIProdDescAttributeInfo&sensTri=AscOperator&searchType=100&viewTaskName=SAQCatalogSearchResultView&pageSize=100");
        }

        public static void DownloadWinePages()
        {
            var web = new WebClient { Encoding = Encoding.UTF8 };
            web.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            for (int i = 0; i < 1000; i = i + 100)
            {
                var req = @"http://www.saq.com/webapp/wcs/stores/servlet/CatalogSearchResultView?storeId=10001&langId=-2&catalogId=10001&searchTerm=&resultCatEntryType=&beginIndex=" + i + "&tri=RechercheUCIProdDescAttributeInfo&sensTri=AscOperator&searchType=100&codeReseau=&categoryId=&viewTaskName=SAQCatalogSearchResultView&catalogVenteId=&pageSize=100";
                System.IO.File.WriteAllText(@"e:\wine\saq_" + i + ".html", web.DownloadString(req), Encoding.UTF8);
            }
        }
    }
}

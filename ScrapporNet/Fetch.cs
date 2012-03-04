using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using HtmlAgilityPack;

namespace ScrapporNet
{
    class Fetch
    {
        public void FetchWinePages()
        {
            var doc = new HtmlDocument();
            doc.Load(@"E:\SourceCode\ScrapporNet\ScrapporNet\bin\Debug\saq_p1.htm",Encoding.UTF8);
            //var wineNames = doc.DocumentNode.SelectNodes("//*/table[@class='recherche']/tbody/tr[*]/td[2]/a");
            var wines = doc.DocumentNode.SelectNodes("//*/table[@class='recherche']/tbody/tr[*]");
            var infos = wines.First().SelectNodes("//td[2]");
            foreach (var info in infos)
            {
                var wineName = info.ChildNodes[1].InnerHtml.Trim(); //Name
                var wineDesc = info.ChildNodes[3].InnerHtml.Replace("\t", "").Replace("\n", "").Replace("&nbsp;", " ").Trim(); //Name

                Console.WriteLine(HttpUtility.HtmlDecode(wineName));
                //Console.WriteLine("---");
                Console.WriteLine("Couleur : " + wineDesc.Split(',')[0]);
                Console.WriteLine("Nature : " + wineDesc.Split(',')[1]);
                Console.WriteLine("Format : " + wineDesc.Split(',')[2]);
                Console.WriteLine("Code SAQ : " + wineDesc.Split(',')[3]);
                //Console.WriteLine(HttpUtility.HtmlDecode(wineDesc));
                Console.WriteLine("---------------------------------");
            }
            
            //doc.Load("http://www.saq.com/webapp/wcs/stores/servlet/CatalogSearchResultView?storeId=10001&catalogId=10001&resultCatEntryType=2&beginIndex=0&tri=RechercheUCIProdDescAttributeInfo&sensTri=AscOperator&searchType=100&viewTaskName=SAQCatalogSearchResultView&pageSize=100");
        }
    }
}

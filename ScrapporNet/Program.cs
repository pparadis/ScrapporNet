using System;
using System.Collections.Generic;
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
            var web = new WebClient {Encoding = Encoding.UTF8};
            for (int i = 0; i < 1000; i= i+100)
            {
                var req = @"http://www.saq.com/webapp/wcs/stores/servlet/CatalogSearchResultView?storeId=10001&langId=-2&catalogId=10001&searchTerm=&resultCatEntryType=&beginIndex="+i+"&tri=RechercheUCIProdDescAttributeInfo&sensTri=AscOperator&searchType=100&codeReseau=&categoryId=&viewTaskName=SAQCatalogSearchResultView&catalogVenteId=&pageSize=100";
                System.IO.File.WriteAllText("saq_"+i+".html", web.DownloadString(req), Encoding.UTF8);
            }
            //web.DownloadFile(req,"test2.html");
            

            //new Fetch().FetchWinePages();

            //var documentStore = new DocumentStore { Url = "http://pascal-pc:8080" };
            //documentStore.Initialize();

            //string wineid;

            ////using (var session = documentStore.OpenSession())
            ////{
            ////    var entity = new Wine { Name = "Allo" };

            ////    session.Store(entity);
            ////    session.SaveChanges();
            ////    wineid = entity.Id;
                
            ////}

            //using (var session = documentStore.OpenSession())
            //{
            //    //var entity = session.Load<Wine>(wineid);
            //    //Console.WriteLine(entity.Name);
            //    //session.SaveChanges();

            //    foreach (var variable in session.Query<Wine>())
            //    {
            //        Console.WriteLine(variable.Id);
            //    }

            //}
            Console.ReadLine();
        }
    }
}

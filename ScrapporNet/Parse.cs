using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using HtmlAgilityPack;
using Raven.Client.Document;
using ScrapporNet.Helpers;

namespace ScrapporNet
{
    public static class Parse
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
                                             Color = wineProperties[0].Trim() ?? "",
                                             Nature = wineProperties[1].Trim() ?? "",
                                             Format = wineProperties[2].Trim() ?? "",
                                             Id = wineProperties[3].Trim() ?? ""

                                         };

                        session.Store(entity);
                    }
                }
                session.SaveChanges();
            }
        }

        public static void ParseWineDetailPages()
        {
            var doc = new HtmlDocument();
            var filename = @"E:\wine\details\M_Montepulciano_d'Abruzzo 2010_ 00518712 .html";
            doc.Load(filename, Encoding.UTF8);

            var documentStore = new DocumentStore
                                    {
                                        ConnectionStringName = "CS"
                                    }.Initialize();

            var wineNameParts = filename.Split('_');

            var wineId = wineNameParts[wineNameParts.Length-1].Replace(".html","").Trim();

            using (var session = documentStore.OpenSession())
            {
                var wine = (from p in session.Query<Wine>()
                            where p.Id == wineId
                            select p).First();

                var rawCup = doc.DocumentNode.SelectNodes("//table[@class='fiche_introduction transparent']/tr/td/p/strong[2]").First().InnerHtml.Replace("\t", "").Replace("\n", "").Replace("\r", "").Replace("&nbsp;", " ").Trim();
                wine.Cup = rawCup.Split(':')[1].TrimStart();
                session.Store(wine);
                session.SaveChanges();
            }

            //Name : doc.DocumentNode.SelectNodes("//table[@class='fiche_introduction transparent']/tr/td/h2")
            //CUP : 
            //Extras infos : doc.DocumentNode.SelectNodes("/html/body/div/div[4]/div/table[2]/tr/td/table/tbody/tr/td")
        }
    }
}

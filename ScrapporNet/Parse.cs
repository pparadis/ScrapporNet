using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using HtmlAgilityPack;
using Raven.Client;
using Raven.Client.Document;
using Raven.Json.Linq;
using ScrapporNet.Entities;
using ScrapporNet.Extensions;
using ScrapporNet.Helpers;

namespace ScrapporNet
{
    public class Parse
    {
        protected IDocumentStore DocumentStore { get; set;}
        public Parse(IDocumentStore documentStore)
        {
            DocumentStore = documentStore;
        }

        public void ParseWinesFromSearchResults()
        {
            var files = GetSearchResultPageList(@"e:\wine\", "*.html");

            var docList = new List<IProduct>();
            foreach (var file in files)
            {
                var doc = new HtmlDocument();
                doc.Load(file, Encoding.UTF8);

                var wineResultsElementList = GetWineResultsElementList(doc);
                foreach (var wineResultElement in wineResultsElementList)
                {
                    var entity = GetWine(wineResultElement);

                    if (entity == null)
                    {
                        continue;
                    }
                    docList.Add(entity);
                }
            }
            SaveWine(docList);
        }

        private static IEnumerable<string> GetSearchResultPageList(string path, string file)
        {
            return Directory.GetFiles(path, file).OrderBy(p => p.ToString(), new NaturalStringComparer());
        }

        protected List<HtmlNode> GetWineResultsElementList(HtmlDocument doc)
        {
            return doc.DocumentNode.SelectNodes("//table[@class='recherche']/tbody/tr[*]/td[2]").ToList();
        }

        protected IProduct GetWine(HtmlNode wineResultElement)
        {
            var wineName = ParseWineName(wineResultElement);
            if(string.IsNullOrEmpty(wineName))
            {
                return null;
            }

            var wineUrl = ParseWineUrl(wineResultElement);
            if (string.IsNullOrEmpty(wineUrl))
            {
                return null;
            }


            var wineDescription = ParseWineDescription(wineResultElement);
            if (wineDescription.Length <= 3)
            {
                return null;
            }

            if (wineDescription[2] == "00002008")
            {
                return null;
            }

            return new Wine
            {
                Name = HttpUtility.HtmlDecode(wineName),
                Url = wineUrl,
                Category = wineDescription[0].Trim() ?? "",
                Nature = wineDescription[1].Trim() ?? "",
                Format = wineDescription[2].Trim() ?? "",
                Id = wineDescription[3].Trim() ?? ""

            };
        }

        private string ParseWineName(HtmlNode info)
        {
            var wineName = info.ChildNodes[1].InnerHtml.Trim();
            return wineName;
        }

        private string ParseWineUrl(HtmlNode info)
        {
            return "http://www.saq.com/webapp/wcs/stores/servlet/" + info.ChildNodes[1].Attributes["href"].Value;
        }

        private string[] ParseWineDescription(HtmlNode info)
        {
            return info.ChildNodes[3].InnerHtml.CleanHtml().Split(',');
        }

        protected void SaveWine(List<IProduct> product)
        {
            using (var session = DocumentStore.OpenSession())
            {
                product.ForEach(session.Store);
                session.SaveChanges();
            }
        }



        public static void ParseWineDetailPages()
        {
            var doc = new HtmlDocument();
            const string filename = @"E:\wine\details\M_Montepulciano_d'Abruzzo 2010_ 00518712 .html";
            doc.Load(filename, Encoding.UTF8);

            var documentStore = new DocumentStore
                                    {
                                        ConnectionStringName = "CS"
                                    }.Initialize();

            var wineNameParts = filename.Split('_');

            var wineId = wineNameParts[wineNameParts.Length - 1].Replace(".html", "").Trim();

            using (var session = documentStore.OpenSession())
            {
                var wine = (session
                                .Query<Wine>()
                                .Where(p => p.Id == wineId))
                                .First();

                wine.Cup = ParseCupCode(doc.DocumentNode);
                wine.Color = ParseColor(doc.DocumentNode);
                wine.Country = ParseCountry(doc.DocumentNode);
                wine.Region = ParseRegion(doc.DocumentNode);
                wine.Appellation = ParseAppellation(doc.DocumentNode);
                wine.Fournisseur = ParseFournisseur(doc.DocumentNode);
                wine.AlcoholRate = ParseAlcohol(doc.DocumentNode);
                wine.Price = ParsePrice(doc.DocumentNode);
                session.Store(wine);
                session.SaveChanges();
            }

            //Name : doc.DocumentNode.SelectNodes("//table[@class='fiche_introduction transparent']/tr/td/h2")
            //Extras infos : doc.DocumentNode.SelectNodes("/html/body/div/div[4]/div/table[2]/tr/td/table/tbody/tr/td")
        }

        private static string ParseCupCode(HtmlNode document)
        {
            const string query = "//table[@class='fiche_introduction transparent']/tr/td/p/strong[2]";
            var rawCup = document.SelectNodes(query).First().InnerHtml.CleanHtml();
            return rawCup.Split(':')[1].TrimStart();
        }

        private static string ParseColor(HtmlNode document)
        {
            const string query = "//table[@id='description-base']/tbody/tr[2]/td[2]";
            return document.SelectNodes(query).First().InnerHtml.CleanHtml();
        }

        private static string ParseCountry(HtmlNode document)
        {
            const string query = "/html[1]/body[1]/div[1]/div[4]/div[1]/table[2]/tr[1]/td[1]/table[1]/tbody/tr[1]/td[2]";
            return document.SelectNodes(query).First().InnerHtml.CleanHtml();
        }

        private static string ParseRegion(HtmlNode document)
        {
            const string query = "/html[1]/body[1]/div[1]/div[4]/div[1]/table[2]/tr[1]/td[1]/table[1]/tbody/tr[2]/td[2]";
            return document.SelectNodes(query).First().InnerHtml.CleanHtml();
        }

        private static string ParseAppellation(HtmlNode document)
        {
            const string query = "/html[1]/body[1]/div[1]/div[4]/div[1]/table[2]/tr[1]/td[1]/table[1]/tbody/tr[3]/td[2]";
            return document.SelectNodes(query).First().InnerHtml.CleanHtml();
        }

        private static string ParseFournisseur(HtmlNode document)
        {
            const string query = "/html[1]/body[1]/div[1]/div[4]/div[1]/table[2]/tr[1]/td[1]/table[1]/tbody/tr[4]/td[2]";
            return document.SelectNodes(query).First().InnerHtml.CleanHtml();
        }

        private static string ParseAlcohol(HtmlNode document)
        {
            const string query = "/html[1]/body[1]/div[1]/div[4]/div[1]/table[2]/tr[1]/td[1]/table[1]/tbody/tr[5]/td[2]";
            return document.SelectNodes(query).First().InnerHtml.CleanHtml();
        }

        private static string ParsePrice(HtmlNode document)
        {
            const string query = "/html[1]/body[1]/div[1]/div[4]/div[1]/table[1]/tr[1]/td[3]/p[1]/span[1]";
            return document.SelectNodes(query).First().InnerHtml.CleanHtml();
        }
    }
}

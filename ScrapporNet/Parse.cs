using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using HtmlAgilityPack;
using Raven.Client.Document;
using ScrapporNet.Entities;
using ScrapporNet.Extensions;
using ScrapporNet.Helpers;

namespace ScrapporNet
{
    public class Parse
    {
        public void ParseWinesFromSearchResults()
        {
            var documentStore = new DocumentStore
                                    {
                                        ConnectionStringName = "CS"
                                    }.Initialize();

            
                var doc = new HtmlDocument();
                var files = Directory.GetFiles(@"e:\wine\", "*.html").OrderBy(p => p.ToString(), new NaturalStringComparer());
                foreach (var file in files)
                {
                    doc.Load(file, Encoding.UTF8);
                    var getWineResultsElementList = GetWineResultsElementList(doc);
                    foreach (var wineResultElement in getWineResultsElementList)
                    {
                        var wineName = ParseWineName(wineResultElement);
                        var wineUrl = ParseWineUrl(wineResultElement);
                        var wineDesc = ParseWineDescription(wineResultElement);
                        var wineProperties = wineDesc.Split(',');

                        if (ProductIsInvalid(wineProperties))
                        {
                            continue;
                        }

                        var entity = new Wine
                                         {
                                             Name = HttpUtility.HtmlDecode(wineName),
                                             Url = wineUrl,
                                             Category = wineProperties[0].Trim() ?? "",
                                             Nature = wineProperties[1].Trim() ?? "",
                                             Format = wineProperties[2].Trim() ?? "",
                                             Id = wineProperties[3].Trim() ?? ""

                                         };
                    using (var session = documentStore.OpenSession())
                    {
                        session.Store(entity);
                        session.SaveChanges();
                    }
                }
            }
        }

        private bool ProductIsInvalid(string[] elementDescriptionList)
        {
            return elementDescriptionList.Length < 3 && elementDescriptionList[2].Trim() == "00002008";
        }

        private string ParseWineDescription(HtmlNode info)
        {
            return info.ChildNodes[3].InnerHtml.CleanHtml();
        }

        private string ParseWineUrl(HtmlNode info)
        {
            return "http://www.saq.com/webapp/wcs/stores/servlet/" + info.ChildNodes[1].Attributes["href"].Value;
        }

        private string ParseWineName(HtmlNode info)
        {
            return info.ChildNodes[1].InnerHtml.Trim();
        }

        private List<HtmlNode> GetWineResultsElementList(HtmlDocument doc)
        {
            return doc.DocumentNode.SelectNodes("//table[@class='recherche']/tbody/tr[*]/td[2]").ToList();
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

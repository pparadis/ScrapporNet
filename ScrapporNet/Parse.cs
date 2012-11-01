using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
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
        private string FilePath { get; set; }

        public Parse(string filePath)
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

        public Parse()
        {
            SetupFolders();
        }

        private void SetupFolders()
        {
            var dateTimeTimeStamp = DateTime.Now.GetTimestamp();
            System.IO.Directory.CreateDirectory(@"e:\Wine\" + dateTimeTimeStamp);
            FilePath = (@"e:\Wine\" + dateTimeTimeStamp + @"\");
        }

        public void ParseWinesFromSearchResults()
        {
            var files = GetFileList(FilePath, "*.html");

            var docList = new List<IProduct>();
            foreach (var file in files)
            {
                var doc = new HtmlDocument();
                doc.Load(file, Encoding.UTF8);

                var wineResultsElementList = GetWineResultsElementList(doc);
                foreach (var wineResultElement in wineResultsElementList)
                {
                    var entity = GetWine(wineResultElement);

                    if (entity == null || entity.Category == "Whisky")
                    {
                        continue;
                    }
                    docList.Add(entity);
                }
            }
            SaveWine(docList);
        }

        private static IEnumerable<string> GetFileList(string path, string file)
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
            if (string.IsNullOrEmpty(wineName))
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
            using (var docStore = new DocumentStore { ConnectionStringName = "CS" }.Initialize())
            {
                using (var session = docStore.OpenSession())
                {
                    product.ForEach(session.Store);
                    session.SaveChanges();
                }
            }
        }

        public void ParseWineDetailPages()
        {
            var doc = new HtmlDocument();
            var files = GetFileList(FilePath + @"\details\", "*.html");

            using (var docStore = new DocumentStore { ConnectionStringName = "CS" }.Initialize())
            {
                foreach (var filename in files)
                {
                    doc.Load(filename, Encoding.UTF8);
                    var wineNameParts = filename.Split('_');
                    var wineId = wineNameParts[wineNameParts.Length - 1].Replace(".html", "").Trim();

                    IQueryable<Wine> wineList;
                    using (var session = docStore.OpenSession())
                    {
                        wineList = (session
                                        .Query<Wine>()
                                        .Where(p => p.Id == wineId));
                    }

                    if (wineList == null || wineList.Count() == 0)
                    {
                        continue;
                    }
                    using (var session = docStore.OpenSession())
                    {
                        var wine = wineList.First();
                        dynamic properties = ParseDescriptiveProperties(doc.DocumentNode);
                        wine.Cup = ParseCupCode(doc.DocumentNode);
                        wine.Color = ParseColor(doc.DocumentNode);
                        wine.Country = properties.Pays;
                        wine.Region = properties.Region;
                        wine.Appellation = properties.Appellation;
                        wine.Fournisseur = properties.Fournisseur;
                        wine.AlcoholRate = properties.PourcentageAlcool;
                        wine.Price = ParsePrice(doc.DocumentNode);
                        session.Store(wine);
                        session.SaveChanges();
                    }
                }
            }
        }

        private string ParseCupCode(HtmlNode document)
        {
            const string query = "//table[@class='fiche_introduction transparent']/tr/td/p/strong[2]";
            var value = document.SelectNodes(query);

            if (value == null)
            {
                return "";
            }

            var rawCup = document.SelectNodes(query).First().InnerHtml.CleanHtml();

            var splitValue = rawCup.Split(':');
            if (splitValue.Count() < 2)
            {
                return "";
            }

            return rawCup.Split(':')[1].TrimStart();
        }


        private static string ParseColor(HtmlNode document)
        {
            const string query = "//table[@id='description-base']/tbody/tr[2]/td[2]";
            var value = document.SelectNodes(query);

            if (value == null)
            {
                return "";
            }

            return value.First().InnerHtml.CleanHtml();
        }

        private ExpandoObject ParseDescriptiveProperties(HtmlNode document)
        {
            dynamic values = new ExpandoObject();
            var elementCountNodes = document.SelectNodes("/html[1]/body[1]/div[1]/div[4]/div[1]/table[2]/tr[1]/td[1]/table[1]/tbody/tr");

            if (elementCountNodes == null)
            {
                return null;
            }
            values.Pays = "";
            values.Region = "";
            values.Appellation = "";
            values.Fournisseur = "";
            values.PourcentageAlcool = "";

            for (var i = 1; i <= elementCountNodes.Count(); ++i)
            {
                var result = document.SelectNodes("/html[1]/body[1]/div[1]/div[4]/div[1]/table[2]/tr[1]/td[1]/table[1]/tbody/tr[" + i + "]/td");
                if (result != null && result.Count == 2)
                {
                    switch (result[0].InnerHtml.CleanHtml().Replace(":", "").Trim())
                    {
                        case "Pays" :
                            values.Pays = result[1].InnerHtml.CleanHtml();
                            break;
                        case "Région" :
                            values.Region = result[1].InnerHtml.CleanHtml();
                            break;
                        case "Appellation":
                            values.Appellation = result[1].InnerHtml.CleanHtml();
                            break;
                        case "Fournisseur":
                            values.Fournisseur = result[1].InnerHtml.CleanHtml();
                            break;
                        case "Pourcentage d'alcool":
                            values.PourcentageAlcool = result[1].InnerHtml.CleanHtml();
                            break;
                    }
                    //Console.WriteLine(result[0].InnerHtml.CleanHtml());
                    //Console.WriteLine(" -- ");
                    //Console.WriteLine(result[1].InnerHtml.CleanHtml());
                }
            }
            return values;
        }

        private static string ParsePrice(HtmlNode document)
        {
            const string query = "/html[1]/body[1]/div[1]/div[4]/div[1]/table[1]/tr[1]/td[3]/p[1]/span[1]";

            var value = document.SelectNodes(query);

            if (value == null)
            {
                return "";
            }

            return value.First().InnerHtml.CleanHtml();
        }
    }
}

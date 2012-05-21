using System.Linq;
using NUnit.Framework;
using Raven.Client.Embedded;
using ScrapporNet;
using ScrapporNet.Entities;
using Assert = NUnit.Framework.Assert;

namespace ParseTests
{
    [TestFixture]
    class SearchPageTests
    {
        private EmbeddableDocumentStore docStore = new EmbeddableDocumentStore {RunInMemory = true};

        public SearchPageTests()
        {
            docStore.Initialize();
            var parse = new Parse(docStore);
            parse.ParseWinesFromSearchResults();
        }
        
        [Test]
        public void SearchResultsParserParserHasParsedMoreThanOneElement()
        {
            // Arrange
            // Act
            // Assert
            using(var session = docStore.OpenSession())
            {
                var item = session.Query<Wine>().Count() > 0;
                Assert.IsTrue(item);
            }
        }

        [Test]
        public void SearchResultsParserParserReturnsASpecificProduct()
        {
            using (var session = docStore.OpenSession())
            {
                var item = session.Query<Wine>().First(p => p.Id == "00518712");
                Assert.IsNotNull(item);
            }
        }

        [Test]
        public void ProductReturnedFromSearchResultsParserIsInTheCorrectFormat()
        {
            using (var session = docStore.OpenSession())
            {
                var item = session.Query<Wine>().Where(p => p.Id == "00518712").First();
                Assert.IsTrue(item.Name == "\"M\" Montepulciano d'Abruzzo 2010");
            }
        }
    }
}

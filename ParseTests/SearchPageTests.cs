using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScrapporNet;

namespace ParseTests
{
    [TestClass]
    class SearchPageTests
    {
        [TestMethod]
        public void FirstSearchResultHasAProductName()
        {
            // Arrange
            //HomeController controller = new HomeController();
            var parse = new Parse();

            //var results = parse.GetWineResultsElementList();

            // Act
            //ViewResult result = controller.Index() as ViewResult;


            // Assert
            //Assert.AreEqual("Welcome to ASP.NET MVC!", result.ViewBag.Message);
        }

    }
}

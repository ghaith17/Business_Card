using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProgressSoft;
using ProgressSoft.Controllers;

namespace ProgressSoft.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void View_business_cards()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.View_business_cards() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void UI()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.UI() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void xml()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.xml() as ViewResult;

            // Assert
           Assert.IsNotNull(result);
        }
        [TestMethod]
        public void csv()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.csv() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void QR()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.QR() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void ExportXML()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            FileResult result = controller.ExportXML() as FileResult;

            // Assert
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void ExportCSV()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            FileResult result = controller.ExportCSV() as FileResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}

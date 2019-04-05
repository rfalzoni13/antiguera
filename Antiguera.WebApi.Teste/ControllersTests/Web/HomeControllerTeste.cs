using Antiguera.WebApi.Controllers.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace Antiguera.WebApi.Teste.ControllersTests.Web
{
    [TestClass]
    public class HomeControllerTeste
    {
        [TestMethod]
        public void Index_View()
        {
            var controller = new HomeController();

            var result = controller.Index() as ViewResult;

            Assert.AreEqual(string.Empty, result.ViewName);
        }
    }
}

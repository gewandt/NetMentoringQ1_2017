using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tasks.AdvancedXml;
using Tasks.AdvancedXML;

namespace Module2.Tests
{
    [TestClass]
    public class ValidationTests
    {
        [TestMethod]
        public void Validate_Valid_Catalog()
        {
            var result = XmlValidator.ValidateXml(PathHelper.ValidXmlPath, PathHelper.BooksXsdPath);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Validate_Invalid_Catalog()
        {
            var result = XmlValidator.ValidateXml(PathHelper.InvalidXmlPath, PathHelper.BooksXsdPath);
            Assert.IsFalse(result);
        }
    }
}

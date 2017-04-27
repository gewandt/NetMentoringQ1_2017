using System;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample03;
using Sample03.E3SClient.Entities;
using Task3.ExtendedLinqProvider;
using Task3.ExtendedLinqProvider.E3SClient;

namespace Module1.Tests
{
	[TestClass]
	public class E3SProviderTests
	{
        [TestMethod]
        public void When_Use_And_Operator()
        {
            //arrange
            var translator = new ExpressionToFTSRequestTranslator();
            Expression<Func<EmployeeEntity, bool>> expr = c => c.workstation.StartsWith("EPBYMINW613") && c.superior.Contains("Bakunovich");
            var expected = "workstation:(EPBYMINW613*) AND superior:(*Bakunovich*)";

            // Act
            var actual = translator.Translate(expr);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void When_Use_Provider()
        {
            //arrange
            var translator = new ExpressionToFTSRequestTranslator();
            Expression<Func<EmployeeEntity, bool>> expr = c => c.workstation == "EPBYMINW6137";
            var expected = "workstation:(EPBYMINW6137)";

            // Act
            var actual = translator.Translate(expr);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Where_StartsWith()
        {
            //arrange
            var translator = new ExpressionToFTSRequestTranslator();
            Expression<Func<EmployeeEntity, bool>> expr = c => c.workstation.StartsWith("EPBYMINW613");
            var expected = "workstation:(EPBYMINW613*)";

            // Act
            var actual = translator.Translate(expr);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Where_EndsWith()
        {
            //arrange
            var translator = new ExpressionToFTSRequestTranslator();
            Expression<Func<EmployeeEntity, bool>> expr = c => c.workstation.EndsWith("BYMINW6137");
            var expected = "workstation:(*BYMINW6137)";

            // Act
            var actual = translator.Translate(expr);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Where_Contains()
        {
            //arrange
            var translator = new ExpressionToFTSRequestTranslator();
            Expression<Func<EmployeeEntity, bool>> expr = c => c.workstation.Contains("BYMINW613");
            var expected = "workstation:(*BYMINW613*)";

            // Act
            var actual = translator.Translate(expr);

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}

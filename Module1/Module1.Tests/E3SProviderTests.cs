using System;
using System.Configuration;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample03;
using Sample03.E3SClient.Entities;
using Task3.ExtendedLinqProvider.E3SClient;

namespace Module1.Tests
{
	[TestClass]
	public class E3SProviderTests
	{
	    private E3SQueryClient _client;
	    private E3SEntitySet<EmployeeEntity> _employees;

        [TestInitialize]
	    public void Init()
	    {
            _client = new E3SQueryClient(
                ConfigurationManager.AppSettings["user"], 
                ConfigurationManager.AppSettings["password"],
                ConfigurationManager.AppSettings["uri"]
                );
            _employees = new E3SEntitySet<EmployeeEntity>(
                ConfigurationManager.AppSettings["user"],
                ConfigurationManager.AppSettings["password"],
                ConfigurationManager.AppSettings["uri"]
                );
	    }

        [TestMethod]
        public void When_Use_And_Operator()
        {
            // Act
            var actual = _employees.Where(c => c.workstation.StartsWith("EPBYMINW613") && c.superior.Contains("Bakunovich")).ToList();
            var expected = _client.SearchFTS<EmployeeEntity>("workstation:(EPBYMINW6137) AND superior:(Bakunovich)", 0, 1).ToList();

            //Assert
            Assert.AreEqual(expected.FirstOrDefault()?.nativename, actual.FirstOrDefault()?.nativename);
        }

        [TestMethod]
        public void When_Use_Provider()
        {
            // Act
            var actual = _employees.Where(c => c.workstation == "EPBYMINW6137").ToList();
            var expected = _client.SearchFTS<EmployeeEntity>("workstation:(EPBYMINW6137)", 0, 1).ToList();

            //Assert
            Assert.AreEqual(expected.FirstOrDefault()?.nativename, actual.FirstOrDefault()?.nativename);
        }

        [TestMethod]
        public void When_Use_Provider_Reversed()
        {
            foreach (var emp in _employees.Where(c => "EPBYMINW6137" == c.workstation))
            {
                Console.WriteLine($"{emp.nativename} {emp.room}");
            }
        }
    }
}

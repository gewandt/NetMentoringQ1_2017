using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Task1.ClassMapper;

namespace Module1.Tests
{
    [TestClass]
    public class MapperTests
    {
        [TestMethod]
        public void Obj_Can_Be_Mapped()
        {
            // Arrange
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Foo, Bar>();

            // Act
            var expectedFoo = new Foo { FirstProp = double.MinValue, SecondProp = string.Empty, ThirdProp = new Guid() };
            var actualBar = mapper.Map(expectedFoo);

            // Assert
            Assert.AreEqual(expectedFoo.FirstProp, actualBar.FirstProp);
            Assert.AreEqual(expectedFoo.SecondProp, actualBar.SecondProp);
            Assert.AreEqual(expectedFoo.ThirdProp, actualBar.ThirdProp);
        }
    }
}

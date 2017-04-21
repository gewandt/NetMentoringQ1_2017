using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Task2.ExpressionTransformation;

namespace Module1.Tests
{
    [TestClass]
    public class ExprTransformationTests
    {
        private BinaryToUnaryTransform _transformBinUnary;
        private ParamToConstTransform _transformParamConst;
        private Dictionary<string, object> _dict;

        [TestInitialize]
        public void Init()
        {
            _dict = new Dictionary<string, object> {{ "a", 1 }};
            _transformBinUnary = new BinaryToUnaryTransform();
            _transformParamConst = new ParamToConstTransform(_dict);
        }

        [TestMethod]
        public void When_Replace_Plus_One_With_Increment()
        {
            // Arrange
            Expression<Func<int, int>> expression = a => a + 1;

            // Act
            var resultExpr = _transformBinUnary.VisitAndConvert(expression, null);
            var expectedResult = expression.Compile().Invoke(1);
            var actualResult = resultExpr.Compile().Invoke(1);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void When_Right_Side_Const_No_Replace()
        {
            // Arrange
            Expression<Func<int, int>> expression = a => 1 + a;

            // Act
            var resultExpr = _transformBinUnary.VisitAndConvert(expression, null);
            var expectedResult = expression.Compile().Invoke(1);
            var actualResult = resultExpr.Compile().Invoke(1);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedResult.ToString(), actualResult.ToString());
        }

        [TestMethod]
        public void When_Multiple_Const_Replace()
        {
            // Arrange
            Expression<Func<int, int>> expression = a => 1 +(a + 1) + 1;

            // Act
            var resultExpr = _transformBinUnary.VisitAndConvert(expression, null);
            var expectedResult = expression.Compile().Invoke(99);
            var actualResult = resultExpr.Compile().Invoke(99);

            Console.WriteLine(expression);
            Console.WriteLine(resultExpr);
            // Assert
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedResult.ToString(), actualResult.ToString());
        }

        [TestMethod]
        public void When_Binary_On_Constants_Replace()
        {
            // Arrange
            Expression<Func<int, int>> expression = a => (1 + 1) + (a + 1);

            // Act
            var resultExpr = _transformBinUnary.VisitAndConvert(expression, null);
            var expectedResult = expression.Compile().Invoke(99);
            var actualResult = resultExpr.Compile().Invoke(99);

            Console.WriteLine(expression);
            Console.WriteLine(resultExpr);
            // Assert
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedResult.ToString(), actualResult.ToString());
        }

        [TestMethod]
        public void When_Replace_Minus_One_With_Decrement()
        {
            // Arrange
            Expression<Func<int, int>> expression = a => a - 1;

            // Act
            var resultExpr = _transformBinUnary.VisitAndConvert(expression, null);
            var expectedResult = expression.Compile().Invoke(1);
            var actualResult = resultExpr.Compile().Invoke(1);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void When_Replace_Param_With_Const()
        {
            // Arrange
            const int param1 = 1, param2 = 2;
            Expression<Func<int, int, int>> expression = (a, b) => a + b;

            // Act
            var resultExpr = _transformParamConst.VisitAndConvert(expression, null);
            var expectedResult = expression.Compile().Invoke(param1, param2);
            var actualResult = resultExpr.Compile().Invoke(param1, param2);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }


        [TestMethod]
        public void When_Replace_Extra_Param_With_Const()
        {
            // Arrange
            Expression<Func<int, int, int, int, int>> expression = (a, b, c, d) => a;

            // Act
            var resultExpr = _transformParamConst.VisitAndConvert(expression, null);
            var expectedResult = expression.Compile().Invoke(1,1,1,1);
            var actualResult = resultExpr.Compile().Invoke(1,1,1,1);

            Console.WriteLine($"Original: {expression}");
            Console.WriteLine($"Modified: {resultExpr}");
            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void When_MultipleSame_Param_With_Const()
        {
            // Arrange
            Expression<Func<int, int>> expression = (a) => a + a + a + a + a + a + a;

            // Act
            var resultExpr = _transformParamConst.VisitAndConvert(expression, null);
            var expectedResult = expression.Compile().Invoke(1);
            var actualResult = resultExpr.Compile().Invoke(1);

            Console.WriteLine($"Original: {expression}");
            Console.WriteLine($"Modified: {resultExpr}");
            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}

using System;
using System.Linq.Expressions;

namespace Task2.ExpressionTransformation
{
    public class BinaryToUnaryTransform : ExpressionVisitor
    {
        private const int ConstValue = 1;

        protected override Expression VisitBinary(BinaryExpression node)
        {
            Console.WriteLine($"Expression: {node}; Left: {node.Left}; Right: {node.Right}");

            var right = node.Right as ConstantExpression;
            if (right != null && ConstValue.Equals(right.Value))
            {
                var visited = Visit(node.Left);
                return GetIncrementDecrement(node.NodeType, visited);
            }

            var left = node.Left as ConstantExpression;
            if (left != null && ConstValue.Equals(left.Value))
            {
                var visited = Visit(node.Right);
                return GetIncrementDecrement(node.NodeType, visited);
            }

            return base.VisitBinary(node);
        }

        private Expression GetIncrementDecrement(ExpressionType type, Expression nodeLeft)
        {
            switch (type)
            {
                case ExpressionType.Add:
                    return Expression.Increment(nodeLeft);
                case ExpressionType.Subtract:
                    return Expression.Decrement(nodeLeft);
                default:
                    return nodeLeft;

            }
        }
    }
}

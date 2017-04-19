using System.Linq.Expressions;

namespace Task2.ExpressionTransformation
{
    public class BinaryToUnaryTransform : ExpressionVisitor
    {
        private const int ConstValue = 1;
        private static readonly ConstantExpression Constant = Expression.Constant(ConstValue);
        private static readonly UnaryExpression IncExp = Expression.Increment(Constant);
        private static readonly UnaryExpression DecExp = Expression.Decrement(Constant);

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (IsRightNodeConstValue(node) == false) return base.VisitBinary(node);
            return GetUnaryExpression(node) ?? base.VisitBinary(node);
        }

        private static UnaryExpression GetUnaryExpression(Expression node)
        {
            if (node.NodeType == ExpressionType.Add) return IncExp;
            return node.NodeType == ExpressionType.Subtract ? DecExp : null;
        }

        private static bool IsRightNodeConstValue(BinaryExpression node)
        {
            var isComplex = node.Left.NodeType == ExpressionType.Add || node.Left.NodeType == ExpressionType.Subtract;
            var constExp = node.Right as ConstantExpression ?? node.Left as ConstantExpression;
            return !isComplex && constExp != null && Equals(constExp.Value, ConstValue);
        }
    }
}

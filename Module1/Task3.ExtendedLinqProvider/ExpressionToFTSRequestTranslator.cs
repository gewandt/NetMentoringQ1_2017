using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Task3.ExtendedLinqProvider
{
    public class ExpressionToFTSRequestTranslator : ExpressionVisitor
    {
        StringBuilder _resultString;

        public string Translate(Expression exp)
        {
            _resultString = new StringBuilder();
            Visit(exp);
            return _resultString.ToString();
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable) && node.Method.Name == "Where")
            {
                var predicate = node.Arguments[1];
                Visit(predicate);
                return node;
            }
            if (node.Method.DeclaringType != typeof (string)) return base.VisitMethodCall(node);
            var memberExpression = node.Object as MemberExpression;
            if (memberExpression == null)
            {
                return base.VisitMethodCall(node);
            }
            var isContains = node.Method.Name == "Contains";
            var before = node.Method.Name == "EndsWith" || isContains;
            var after = node.Method.Name == "StartsWith" || isContains;
            if (after || before)
            {
                Visit(memberExpression);
                _resultString.Append("(");
                if (before) _resultString.Append("*");
                Visit(node.Arguments[0]);
                if (after) _resultString.Append("*");
                _resultString.Append(")");
                return node;
            }
            return base.VisitMethodCall(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    Expression memberAccess = null;
                    Expression constantExpression = null;

                    if (node.Left.NodeType == ExpressionType.MemberAccess)
                    {
                        memberAccess = node.Left;
                        constantExpression = node.Right.NodeType == ExpressionType.Constant ? node.Right : null;
                    }
                    else if (node.Right.NodeType == ExpressionType.MemberAccess)
                    {
                        memberAccess = node.Right;
                        constantExpression = node.Left.NodeType == ExpressionType.Constant ? node.Left : null;
                    }
                    if (memberAccess == null) throw new NotSupportedException("Property is not supported.");
                    if (constantExpression == null) throw new NotSupportedException("Constant is not supported.");
                    Visit(memberAccess);
                    _resultString.Append("(");
                    Visit(constantExpression);
                    _resultString.Append(")");
                    break;
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    Visit(node.Left);
                    _resultString.Append(" AND ");
                    Visit(node.Right);
                    break;
                default:
                    throw new NotSupportedException($"Operation {node.NodeType} is not supported");
            }
            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _resultString.Append(node.Member.Name).Append(":");
            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _resultString.Append(node.Value);
            return node;
        }
    }
}

using System.Collections.Generic;
using System.Linq.Expressions;

namespace Task2.ExpressionTransformation
{
    public class ParamToConstTransform : ExpressionVisitor
    {
        private readonly Dictionary<string, object> _initDict;

        public ParamToConstTransform(Dictionary<string, object> inputDict)
        {
            _initDict = inputDict ?? new Dictionary<string, object>();
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            object param;
            return _initDict.TryGetValue(node.Name, out param) ? Expression.Constant(param, node.Type) : base.VisitParameter(node);
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            var body = Visit(node.Body);
            return Expression.Lambda(body, node.Parameters);
        }
    }
}

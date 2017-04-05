using Sample03.E3SClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Task3.ExtendedLinqProvider;
using Task3.ExtendedLinqProvider.E3SClient;

namespace Sample03
{
	public class E3SLinqProvider : IQueryProvider
	{
		private E3SQueryClient e3sClient;

		public E3SLinqProvider(E3SQueryClient client)
		{
			e3sClient = client;
		}

		public IQueryable CreateQuery(Expression expression)
		{
			throw new NotImplementedException();
		}

		public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
		{
			return new E3SQuery<TElement>(expression, this);
		}

		public object Execute(Expression expression)
		{
			throw new NotImplementedException();
		}

		public TResult Execute<TResult>(Expression expression)
		{
			var itemType = TypeHelper.GetElementType(expression.Type);

			var translator = new ExpressionToFTSRequestTranslator();
			var queryString = translator.Translate(expression);

			return (TResult)(e3sClient.SearchFTS(itemType, queryString));
		}
	}
}

using Sample03.E3SClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Task3.ExtendedLinqProvider.E3SClient;

namespace Sample03
{
	public class E3SEntitySet<T> : IQueryable<T> where T : E3SEntity
	{
		protected Expression expression;
		protected IQueryProvider provider;

		public E3SEntitySet(string user, string password, string uri)
		{
			expression = Expression.Constant(this);

			var client = new E3SQueryClient(user, password, uri);

			provider = new E3SLinqProvider(client);
		}

		public Type ElementType
		{
			get
			{
				return typeof(T);
			}
		}

		public Expression Expression
		{
			get
			{
				return expression;
			}
		}

		public IQueryProvider Provider
		{
			get
			{
				return provider;
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			return provider.Execute<IEnumerable<T>>(expression).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return provider.Execute<IEnumerable>(expression).GetEnumerator();
		}
	}
}

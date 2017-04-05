using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Sample03.E3SClient;

namespace Task3.ExtendedLinqProvider.E3SClient
{
	public class E3SQueryClient
	{
		private readonly string _userName;
		private readonly string _password;
		private readonly Uri _uri;

		public E3SQueryClient(string user, string password, string uri)
		{
		    _uri = new Uri(uri);
            _userName = user;
			_password = password;
		}

		public IEnumerable<T> SearchFTS<T>(string query, int start = 0, int limit = 10) where T : E3SEntity
		{
		    var resultString = GetResultString(typeof(T), query, start, limit);
		    return JsonConvert.DeserializeObject<FTSResponse<T>>(resultString).items.Select(t => t.data);
		}

	    public IEnumerable SearchFTS(Type type, string query, int start = 0, int limit = 10)
		{
			var result = GetResultString(type, query, start, limit);
	        var genericType = typeof(FTSResponse<>).MakeGenericType(type);
			var resultJson = JsonConvert.DeserializeObject(result, genericType);
			var list = Activator.CreateInstance(typeof(List<>).MakeGenericType(type)) as IList;
			foreach (var item in (IEnumerable)genericType.GetProperty("items").GetValue(resultJson))
			{
				list?.Add(item.GetType().GetProperty("data").GetValue(item));
			}
			return list;
		}

	    private string GetResultString(Type type, string query, int start, int limit)
	    {
	        var client = CreateClient();
	        var requestGenerator = new FTSRequestGenerator(_uri);
	        var request = requestGenerator.GenerateRequestUrl(type, query, start, limit);
	        var resultString = client.GetStringAsync(request).Result;
	        return resultString;
	    }

	    private HttpClient CreateClient()
		{
			var client = new HttpClient(new HttpClientHandler
			{
				AllowAutoRedirect = true,
				PreAuthenticate = true
			});
			var authHeader = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(new ASCIIEncoding().GetBytes($"{_userName}:{_password}")));
			client.DefaultRequestHeaders.Authorization = authHeader;
			return client;
		}
	}
}

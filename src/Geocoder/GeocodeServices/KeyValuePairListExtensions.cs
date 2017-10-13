using System;
using System.Collections.Generic;
using System.Web;

namespace Geo.Coder.GeocodeServices
{
	public static class KeyValuePairListExtensions
	{
		public static string ToQueryString(this IEnumerable<KeyValuePair<string, string>> parameters)
		{
			var uriBuilder = new UriBuilder();
			var query = HttpUtility.ParseQueryString(uriBuilder.Query);
			foreach (var urlParameter in parameters)
			{
				query[urlParameter.Key] = urlParameter.Value;
			}
			uriBuilder.Query = query.ToString();
			return uriBuilder.Query;
		}
	}
}
using System;
using System.Collections.Generic;
using System.Web;

namespace Geocoder.GeocodeServices
{
	static class Class1
	{
		private static string ToQueryString(this IEnumerable<KeyValuePair<string, string>> parameters)
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
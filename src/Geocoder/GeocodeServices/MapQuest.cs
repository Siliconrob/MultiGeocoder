using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using NLog;

namespace Geo.Coder.GeocodeServices
{
	public class MapQuestGeoCoder : IGeocode
	{
		private readonly Logger _log;

		public MapQuestGeoCoder()
		{
			_log = LogManager.GetLogger(nameof(MapQuestGeoCoder));
		}

		public string BaseUrl => @"http://www.mapquestapi.com/";

		public string GeoCodeRestUrl => @"geocoding/v1/address";

		public async Task<string> Find(ApiGeocodeQuery queryParams)
		{
			var watch = Stopwatch.StartNew();

			var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };
			var query = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("key", queryParams.Key),
				new KeyValuePair<string, string>("location", queryParams.Q),
			}.ToQueryString();
			var request = new HttpRequestMessage(HttpMethod.Get, GeoCodeRestUrl + query);

			_log.Debug("Request constructed '{0}' {1} milliseconds", watch.ElapsedMilliseconds);

			var geoResponse = await client.SendAsync(request).ConfigureAwait(false);

			var responseString = await geoResponse.Content.ReadAsStringAsync();
			_log.Debug("Geocode response: {0}", responseString);

			return responseString;

		}
	}
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

using NLog;

namespace Geo.Coder.GeocodeServices
{
	public class BingGeoCoder : IGeocode
	{
		private readonly Logger _log;


		public BingGeoCoder()
		{
			this._log = LogManager.GetLogger(nameof(BingGeoCoder));
		}


		public string BaseUrl => @"http://dev.virtualearth.net";

		public string GeoCodeRestUrl => @"REST/v1/Locations";


		public async Task<string> Find(ApiGeocodeQuery queryParams)
		{
			var watch = Stopwatch.StartNew();
			var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };
			var query = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>(nameof(queryParams.Key), queryParams.Key),
				new KeyValuePair<string, string>(nameof(queryParams.App), queryParams.App),
				new KeyValuePair<string, string>(nameof(queryParams.Q), queryParams.Q)
			}.ToQueryString();
			var request = new HttpRequestMessage(HttpMethod.Get, GeoCodeRestUrl + query);

			_log.Debug("Request constructed '{0}' {1} milliseconds", watch.ElapsedMilliseconds);

			var geoResponse = await client.SendAsync(request).ConfigureAwait(false);

			var responseString = await geoResponse.Content.ReadAsStringAsync();
			_log.Debug("Geocode response: {0}", responseString);

			return  responseString;
		}
	}
}
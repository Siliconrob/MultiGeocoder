using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using NLog;

namespace Geo.Coder.GeocodeServices
{
	public class NokiaGeoCoder : IGeocode
	{
		private readonly Logger _log;

		public NokiaGeoCoder()
		{
			_log = LogManager.GetLogger(nameof(NokiaGeoCoder));
		}

		public string BaseUrl => @"http://geocoder.cit.api.here.com/";

		public string GeoCodeRestUrl => @"6.2/geocode.json";

		public async Task<string> Find(ApiGeocodeQuery queryParams)
		{
			var watch = Stopwatch.StartNew();


			var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };
			var query= new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("searchtext", queryParams.Q),
				new KeyValuePair<string, string>("app_id", queryParams.App),
				new KeyValuePair<string, string>("app_code", queryParams.Key)
			}.ToQueryString();
			var request = new HttpRequestMessage(HttpMethod.Get, GeoCodeRestUrl+query)
			;

			_log.Debug("Request constructed '{0}' {1} milliseconds", watch.ElapsedMilliseconds);

			var geoResponse = await client.SendAsync(request).ConfigureAwait(false);

			var responseString = await geoResponse.Content.ReadAsStringAsync();
			_log.Debug("Geocode response: {0}", responseString);

			return responseString;
		}
	}
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Geocoder.GeocodeServices;

using NLog;

namespace Geo.Coder.GeocodeServices
{
	public class GoogleGeoCoder : IGeocode
	{
		private readonly Logger _log;


		public GoogleGeoCoder()
		{
			this._log = LogManager.GetLogger(nameof(GoogleGeoCoder));
		}


		public string BaseUrl => @"https://maps.googleapis.com";

		public string GeoCodeRestUrl => @"maps/api/geocode/json";


		public async Task<string> Find(ApiGeocodeQuery queryParams)
		{
			var watch = Stopwatch.StartNew();

			var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };
			var query = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("address", queryParams.Q),
				new KeyValuePair<string, string>("sensor", "false")
			}.ToQueryString();
			var request = new HttpRequestMessage(HttpMethod.Get, GeoCodeRestUrl + query);
			
			

			this._log.Debug("Request constructed '{0}' {1} milliseconds", watch.ElapsedMilliseconds);

			var geoResponse = await client.SendAsync(request).ConfigureAwait(false);

			var responseString = await geoResponse.Content.ReadAsStringAsync();
			this._log.Debug("Geocode response: {0}", responseString);

			return responseString;
		}
	}

	public static class OutputFormats
	{
		public const string Json = @"json";
		public const string Xml = @"xml";
	}
}
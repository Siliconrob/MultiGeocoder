using System.Diagnostics;
using System.Threading.Tasks;
using NLog;
using RestSharp;

namespace Geo.Coder.GeocodeServices
{
  public class GoogleGeoCoder : IGeocode
  {
    private readonly Logger _log;

    public GoogleGeoCoder()
    {
      _log = LogManager.GetLogger(nameof(GoogleGeoCoder));
    }

    public string BaseUrl => @"https://maps.googleapis.com";

    public string GeoCodeRestUrl => @"maps/api/geocode/{0}";

    public async Task<string> Find(ApiGeocodeQuery queryParams)
    {
      var watch = Stopwatch.StartNew();
      var client = new RestClient(BaseUrl);

      var googleQuery = new
      {
        address = queryParams.Q,
        sensor = false.ToString().ToLowerInvariant()
      };

      var request = new RestRequest(string.Format(GeoCodeRestUrl, OutputFormats.Json));
      request.AddObject(googleQuery);

      _log.Debug("Request constructed '{0}' {1} milliseconds", client.BuildUri(request), watch.ElapsedMilliseconds);
      var geoResponse = await client.ExecuteAsGet(request, HttpMethods.Get).GetTaskResult();
      _log.Debug("Geocode response: {0}", geoResponse.Content);

      return geoResponse.Content;
    }
  }

  public static class OutputFormats
  {
    public const string Json = @"json";
    public const string Xml = @"xml";
  }
}
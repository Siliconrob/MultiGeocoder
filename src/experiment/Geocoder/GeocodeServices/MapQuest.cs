using System.Diagnostics;
using System.Threading.Tasks;
using NLog;
using RestSharp;

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
      var client = new RestClient(BaseUrl);
      var request = new RestRequest(GeoCodeRestUrl);

      request.AddParameter("key", queryParams.Key);
      request.AddParameter("location", queryParams.Q);

      _log.Debug("Request constructed '{0}' {1} milliseconds", client.BuildUri(request), watch.ElapsedMilliseconds);
      var geoResponse = await client.ExecuteAsGet(request, HttpMethods.Get).GetTaskResult();
      _log.Debug("Geocode response: {0}", geoResponse.Content);

      return geoResponse.Content;
    }
  }
}
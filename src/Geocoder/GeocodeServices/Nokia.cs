using System.Diagnostics;
using System.Threading.Tasks;
using NLog;
using RestSharp;

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
      var client = new RestClient(BaseUrl);

      var nokiaQuery = new
      {
        searchtext = queryParams.Q,
        app_id = queryParams.App,
        app_code = queryParams.Key
      };

      var request = new RestRequest(GeoCodeRestUrl);
      request.AddObject(nokiaQuery);

      _log.Debug("Request constructed '{0}' {1} milliseconds", client.BuildUri(request), watch.ElapsedMilliseconds);
      var geoResponse = await client.ExecuteAsGet(request, HttpMethods.Get).GetTaskResult();
      _log.Debug("Geocode response: {0}", geoResponse.Content);
      return geoResponse.Content;
    }
  }
}
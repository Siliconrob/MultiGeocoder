using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Geo.Coder;
using Geo.Coder.GeocodeServices;
using ServiceStack;

namespace Geo.Cacher
{
  public class BatchGeocode : Service
  {
    private async Task<string> Geocode(string freeFormAddress, string apiKey)
    {
      var log = this.Log();
      var cacheKey = freeFormAddress.Trim().ToLowerInvariant();
      log.DebugFormat("Cache Id: {0}", cacheKey);

      Func<string> fn = () =>
      {
        var nokiaHere = new NokiaGeoCoder();
        return
          Task.Run(async () => await nokiaHere.Find(new ApiGeocodeQuery {Q = freeFormAddress, Key = apiKey})).Result;
      };
      return await Cache.ToResultUsingCache(cacheKey, fn).AsTaskResult();
    }

    public object Get(AddressesRequest request)
    {
      var results = new ConcurrentQueue<string>();
      Parallel.ForEach(request.FreeformAddresses, z => results.Enqueue(Geocode(z, request.ApiKey).Result));
      return results;
    }
  }
}
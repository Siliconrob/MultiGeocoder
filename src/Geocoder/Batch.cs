using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog;

namespace Geo.Coder
{
  public static class Batch
  {
    public static async Task<IEnumerable<string>> Geocode(IEnumerable<string> addresses,
      Func<string, Task<string>> geocode)
    {
      var batchResults = await Task.Run(() =>
      {
        var results = new ConcurrentQueue<string>();
        Parallel.ForEach(addresses, async freeFormAddress =>
        {
          try
          {
            var geoResult = await geocode(freeFormAddress);
            results.Enqueue(geoResult);
          }
          catch (Exception ae)
          {
            var log = LogManager.GetLogger(nameof(Batch));
            log.Error("Geocode failure {0}{1}", Environment.NewLine, ae.Message);
          }
        });
        return results;
      });
      return batchResults;
    }
  }
}
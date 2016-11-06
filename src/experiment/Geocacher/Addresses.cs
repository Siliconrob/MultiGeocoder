using System.Collections.Generic;
using ServiceStack;

namespace Geo.Cacher
{
  [Route("/geocode/{ApiKey}/{FreeformAddresses}", "GET")]
  public class AddressesRequest
  {
    public string ApiKey { get; set; }
    public List<string> FreeformAddresses { get; set; }
  }
}
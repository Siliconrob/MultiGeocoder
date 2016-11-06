using System.Threading.Tasks;

namespace Geo.Coder
{
  public interface IGeocode
  {
    string BaseUrl { get; }
    string GeoCodeRestUrl { get; }
    Task<string> Find(ApiGeocodeQuery queryParams);
  }
}
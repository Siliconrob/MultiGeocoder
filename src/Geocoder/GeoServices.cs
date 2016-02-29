using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Geo.Coder.GeocodeServices;
using Geo.Formats.JSON.Bing;
using Geo.Formats.JSON.Google;
using Geo.Formats.JSON.MapQuest;
using Geo.Formats.JSON.Nokia;
using System.Linq;
using Geo.Keys;
using Newtonsoft.Json;
using NLog;

namespace Geo.Coder
{
  public static class GeoServices
  {
    private const string LatLngFmt = @"Address submitted '{0}' Geocode Result ({1}, {2})";
    public static readonly ILogger Log = LogManager.GetLogger(nameof(GeoServices));

    public static List<Func<string, Task<string>>> All()
    {
      return new List<Func<string, Task<string>>>
      {
        MapQuest,
        Bing,
        Google,
        Nokia
      };
    }

    public static async Task<string> Nokia(string freeFormAddress)
    {
      var g = new NokiaGeoCoder();
      var jsonContent = await g.Find(new ApiGeocodeQuery {Q = freeFormAddress, Key = Api.Nokia.Key, App = Api.Nokia.Id});

      var data =  jsonContent.FromJson<NokiaJson>();
      var result = data.Response.View.FirstSafe() ?? new View
      {
        Result = new List<Formats.JSON.Nokia.Result>
        {
          new Formats.JSON.Nokia.Result
          {
            Location = new Formats.JSON.Nokia.Location
            {
              DisplayPosition = new DisplayPosition()
            }
          }
        }
      };
      var latlng = result.Result.FirstSafe().Location.DisplayPosition;
      return string.Format(LatLngFmt, freeFormAddress, latlng.Latitude, latlng.Longitude);
    }

    public static async Task<string> Google(string freeFormAddress)
    {
      var g = new GoogleGeoCoder();
      var jsonContent = await g.Find(new ApiGeocodeQuery {Q = freeFormAddress});
      var data = jsonContent.FromJson<GoogleJson>();
      var results = data.Results.FirstSafe() ?? new Formats.JSON.Google.Result
      {
        Geometry = new Geometry
        {
          Location = new Formats.JSON.Google.Location()
        }
      };
      var latlng = results.Geometry.Location;
      return string.Format(LatLngFmt, freeFormAddress, latlng.Lat, latlng.Lng);
    }

    public static async Task<string> MapQuest(string freeFormAddress)
    {
      var g = new MapQuestGeoCoder();
      var jsonContent = await g.Find(new ApiGeocodeQuery {Q = freeFormAddress, Key = Api.Mapquest});
      var data = jsonContent.FromJson<MapQuestJson>();

      var result = data.Results.FirstSafe() ?? new Formats.JSON.MapQuest.Result
      {
        Locations = new List<Formats.JSON.MapQuest.Location>
        {
          new Formats.JSON.MapQuest.Location()
        }
      };
      var latlng = result.Locations.FirstSafe().LatLng;
      return string.Format(LatLngFmt, freeFormAddress, latlng.Lat, latlng.Lng);
    }

    public static async Task<string> Bing(string freeFormAddress)
    {
      var g = new BingGeoCoder();
      var jsonContent = await g.Find(new ApiGeocodeQuery {Q = freeFormAddress, Key = Api.Bing});
      var data = jsonContent.FromJson<BingJson>();
      var foundResource = data.ResourceSets.FirstSafe().Resources.FirstSafe() ?? new Resource
      {
        GeocodePoints = new List<GeocodePoint>
        {
          new GeocodePoint
          {
            Coordinates = new List<double>()
          }
        }
      };
      var latlng = foundResource.GeocodePoints.FirstSafe().Coordinates;
      return string.Format(LatLngFmt, freeFormAddress, latlng.FirstOrDefault(), latlng.LastOrDefault());
    }
  }

  public static class StringExtensions
  {
    public static T FromJson<T>(this string jsonContent)
    {
      return JsonConvert.DeserializeObject<T>(jsonContent);
    }

    public static string ToJson<T>(this T input)
    {
      return JsonConvert.SerializeObject(input);
    }
  }

}
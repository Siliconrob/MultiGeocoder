using System.Collections.Generic;

/// Generated classes from JSON results
/// http://json2csharp.com/

namespace Geo.Formats.JSON.MapQuest
{
  public class LatLng
  {
    public double Lng { get; set; }
    public double Lat { get; set; }
  }

  public class DisplayLatLng
  {
    public double Lng { get; set; }
    public double Lat { get; set; }
  }

  public class Location
  {
    public LatLng LatLng { get; set; }
    public string AdminArea4 { get; set; }
    public string AdminArea5Type { get; set; }
    public string AdminArea4Type { get; set; }
    public string AdminArea5 { get; set; }
    public string Street { get; set; }
    public string AdminArea1 { get; set; }
    public string AdminArea3 { get; set; }
    public string Type { get; set; }
    public DisplayLatLng DisplayLatLng { get; set; }
    public int LinkId { get; set; }
    public string PostalCode { get; set; }
    public string SideOfStreet { get; set; }
    public bool DragPoint { get; set; }
    public string AdminArea1Type { get; set; }
    public string GeocodeQuality { get; set; }
    public string GeocodeQualityCode { get; set; }
    public string MapUrl { get; set; }
    public string AdminArea3Type { get; set; }
  }

  public class ProvidedLocation
  {
    public string Location { get; set; }
  }

  public class Result
  {
    public List<Location> Locations { get; set; }
    public ProvidedLocation ProvidedLocation { get; set; }
  }

  public class Options
  {
    public bool IgnoreLatLngInput { get; set; }
    public int MaxResults { get; set; }
    public bool ThumbMaps { get; set; }
  }

  public class Copyright
  {
    public string Text { get; set; }
    public string ImageUrl { get; set; }
    public string ImageAltText { get; set; }
  }

  public class Info
  {
    public Copyright Copyright { get; set; }
    public int Statuscode { get; set; }
    public List<object> Messages { get; set; }
  }

  public class MapQuestJson
  {
    public List<Result> Results { get; set; }
    public Options Options { get; set; }
    public Info Info { get; set; }
  }
}
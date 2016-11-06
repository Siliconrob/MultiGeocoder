using System.Collections.Generic;

/// Generated classes from JSON results
/// http://json2csharp.com/

namespace Geo.Formats.JSON.Nokia
{
  public class MetaInfo
  {
    public string Timestamp { get; set; }
  }

  public class MatchQuality
  {
    public double State { get; set; }
    public double City { get; set; }
    public List<double> Street { get; set; }
    public double HouseNumber { get; set; }
    public double PostalCode { get; set; }
  }

  public class DisplayPosition
  {
    public double Latitude { get; set; }
    public double Longitude { get; set; }
  }

  public class NavigationPosition
  {
    public double Latitude { get; set; }
    public double Longitude { get; set; }
  }

  public class TopLeft
  {
    public double Latitude { get; set; }
    public double Longitude { get; set; }
  }

  public class BottomRight
  {
    public double Latitude { get; set; }
    public double Longitude { get; set; }
  }

  public class MapView
  {
    public TopLeft TopLeft { get; set; }
    public BottomRight BottomRight { get; set; }
  }

  public class AdditionalData
  {
    public string Value { get; set; }
    public string Key { get; set; }
  }

  public class Address
  {
    public string Label { get; set; }
    public string Country { get; set; }
    public string State { get; set; }
    public string County { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string HouseNumber { get; set; }
    public string PostalCode { get; set; }
    public List<AdditionalData> AdditionalData { get; set; }
  }

  public class Location
  {
    public string LocationId { get; set; }
    public string LocationType { get; set; }
    public DisplayPosition DisplayPosition { get; set; }
    public List<NavigationPosition> NavigationPosition { get; set; }
    public MapView MapView { get; set; }
    public Address Address { get; set; }
  }

  public class Result
  {
    public double Relevance { get; set; }
    public string MatchLevel { get; set; }
    public MatchQuality MatchQuality { get; set; }
    public string MatchType { get; set; }
    public Location Location { get; set; }
  }

  public class View
  {
    public string Type { get; set; }
    public int ViewId { get; set; }
    public List<Result> Result { get; set; }
  }

  public class Response
  {
    public MetaInfo MetaInfo { get; set; }
    public List<View> View { get; set; }
  }

  public class NokiaJson
  {
    public Response Response { get; set; }
  }
}
namespace Geo.Keys
{
  /// <summary>
  ///   All of these keys are restricted from use in production mode.
  /// </summary>
  public static class Api
  {
    public static string Bing
    {
      get
      {
        var apiKey = AppSettings.Get(@"BING");
        return apiKey;
      }
    }

    public static string Mapquest
    {
      get
      {
        var apiKey = AppSettings.Get(@"MAPQUEST");
        return apiKey;
      }
    }

    public static NokiaApi Nokia => new NokiaApi
    {
      Id = AppSettings.Get(@"NOKIA_ID"),
      Key = AppSettings.Get(@"NOKIA_KEY")
    };
  }

  public class NokiaApi
  {
    public string Key { get; set; }
    public string Id { get; set; }
  }
}
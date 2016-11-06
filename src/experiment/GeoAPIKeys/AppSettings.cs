using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;

namespace Geo.Keys
{
  public static class AppSettings
  {
    public static string Get(string keyName, string defaultValue = "")
    {
      defaultValue = defaultValue ?? "";
      var appSettings = ConfigurationManager.AppSettings.ToDictionary();
      var matches = appSettings.Where(z => string.Equals(z.Key, keyName, StringComparison.InvariantCultureIgnoreCase));
      var firstMatch = matches.FirstOrDefault();
      return firstMatch.Value ?? defaultValue;
    }
  }

  public static class NameValueCollectionExtensions
  {
    public static ReadOnlyDictionary<string, string> ToDictionary(this NameValueCollection collection)
    {
      if (collection == null)
      {
        throw new ArgumentNullException(nameof(collection));
      }
      return new ReadOnlyDictionary<string, string>(
        collection.AllKeys.ToDictionary(key => key, key => collection[key]));
    }
  }
}
using System.Collections.Generic;
using System.Linq;

namespace Geo.Coder
{
  public static class ListExtensions
  {
    public static T FirstSafe<T>(this IEnumerable<T> currentList) where T : new()
    {
      var empty = new T();
      var actualItems = currentList ?? new List<T>();
      if (!actualItems.Any())
      {
        return empty;
      }
      var emptyItem = empty.ToJson();
      foreach (var item in actualItems.Where(item => item != null).Where(item => !string.Equals(item.ToJson(), emptyItem)))
      {
        return item;
      }
      return empty;
    }

    public static IEnumerable<IEnumerable<TSource>> Chunk<TSource>(this IEnumerable<TSource> source, int chunkSize)
    {
      var enumerable = source as TSource[] ?? source.ToArray();
      for (var i = 0; i < enumerable.Count(); i += chunkSize)
      {
        yield return enumerable.Skip(i).Take(chunkSize);
      }
    }
  }
}
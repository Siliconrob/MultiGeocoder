using System.Collections.Generic;
using System.Linq;

namespace Geo.Coder
{
  public static class ListExtensions
  {
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
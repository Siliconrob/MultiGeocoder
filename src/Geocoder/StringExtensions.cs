using Newtonsoft.Json;

namespace Geo.Coder
{
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
using Geo.Keys;

using Microsoft.Extensions.Configuration;

namespace GeoKeys
{
	public static class Api
	{
		public static readonly ApiOptions ApiOptions;
		public static readonly NokiaApiOptions NokiaApiOptions;


		static Api()
		{
			var builder = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json");

			IConfiguration configuration = builder.Build();
			ApiOptions = new ApiOptions
			{
				Bing = configuration["BING"],
				Mapquest = configuration["MAPREQUEST"]
			};
			NokiaApiOptions = new NokiaApiOptions
			{
				Id = configuration["NOKIA_ID"],
				Key = configuration["NOKIA_KEY"]
			};
		}
	}
}
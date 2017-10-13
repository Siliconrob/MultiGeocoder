using Microsoft.Extensions.Configuration;

namespace Geo.Options
{
	public static class Options
	{
		public static readonly string Bing;
		public static readonly string Mapquest;
		public static readonly string NokiaId;
		public static readonly string NokiaKey;


		static Options()
		{
			var builder = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json");

			IConfiguration configuration = builder.Build();

			Bing = configuration["BING"];
			Mapquest = configuration["MAPREQUEST"];

			NokiaId = configuration["NOKIA_ID"];
			NokiaKey = configuration["NOKIA_KEY"];
		}
	}
}
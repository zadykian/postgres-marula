using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Infrastructure.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="IHostBuilder"/> type.
	/// </summary>
	public static class HostBuilderExtensions
	{
		/// <summary>
		/// Add json file named <paramref name="jsonFileName"/> to app configuration.
		/// </summary>
		public static IHostBuilder AddJsonConfig(this IHostBuilder hostBuilder, NonEmptyString jsonFileName)
			=> hostBuilder
				.ConfigureAppConfiguration((context, builder) => builder
					.AddJsonFile(
						path: $"{jsonFileName}.json",
						optional: false,
						reloadOnChange: true)
					.AddJsonFile(
						path: $"{jsonFileName}.{context.HostingEnvironment.EnvironmentName}.json",
						optional: true,
						reloadOnChange: true));
	}
}
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Postgres.Marula.WebApi.Common
{
	/// <summary>
	/// Extension methods for <see cref="IHostBuilder"/> type.
	/// </summary>
	public static class HostBuilderExtensions
	{
		/// <summary>
		/// Configure <paramref name="hostBuilder"/> for hosting web application with default startup.
		/// </summary>
		public static IHostBuilder WebWithDefaultStartup(this IHostBuilder hostBuilder)
			=> hostBuilder.ConfigureWebHostDefaults(builder => builder.UseStartup<DefaultStartup>());
	}
}
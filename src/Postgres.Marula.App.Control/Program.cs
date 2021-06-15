using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.Hosting;

namespace Postgres.Marula.App.Control
{
	/// <summary>
	/// Application entry point.
	/// </summary>
	internal static class Program
	{
		/// <summary>
		/// Entry point method.
		/// </summary>
		private static Task Main(string[] args)
			=> CommonHostBuilder
				.WithJsonConfig(args, "marula-control-config")
				.ConfigureLogging(builder =>
				{
					// todo: and in-memory logging
					builder.ClearProviders();
				})
				.ConfigureServices(services => services.AddComponent<ControlAppComponent>())
				.RunConsoleAsync();
	}
}
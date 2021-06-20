using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Infrastructure;
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
				.ConfigureServices(AddComponents)
				.RunConsoleAsync();

		/// <summary>
		/// Add all required components to <paramref name="services"/>. 
		/// </summary>
		private static void AddComponents(IServiceCollection services)
			=> services
				.AddComponent<ControlAppComponent>()
				.AddComponent<InfrastructureAppComponent>();
	}
}
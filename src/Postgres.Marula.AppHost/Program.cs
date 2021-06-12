using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Postgres.Marula.Calculations;
using Postgres.Marula.DatabaseAccess;
using Postgres.Marula.Infrastructure;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.AppHost
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
				.WithConfig(args, "marula-app-config")
				.ConfigureServices(AddComponents)
				.Build()
				.RunAsync();

		/// <summary>
		/// Add all required components to <paramref name="services"/>. 
		/// </summary>
		private static void AddComponents(IServiceCollection services)
			=> services
				.AddComponent<InfrastructureAppComponent>()
				.AddComponent<DatabaseAccessAppComponent>()
				.AddComponent<CalculationsAppComponent>()
				.AddComponent<AppHostAppComponent>();
	}
}
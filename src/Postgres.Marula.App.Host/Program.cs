using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Postgres.Marula.Calculations;
using Postgres.Marula.DatabaseAccess;
using Postgres.Marula.Infrastructure;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.Hosting;
using Postgres.Marula.WebApi.Common;

namespace Postgres.Marula.App.Host
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
				.WithJsonConfig(args, "marula-host-config")
				.WebWithDefaultStartup()
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
				.AddComponent<HostAppComponent>();
	}
}
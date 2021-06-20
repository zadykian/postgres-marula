using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Postgres.Marula.HttpApi.Common;
using Postgres.Marula.HwInfo;
using Postgres.Marula.Infrastructure;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.Hosting;

namespace Postgres.Marula.App.Agent
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
				.WithJsonConfig(args, "marula-agent-config")
				.EnableHttpApi()
				.ConfigureServices(AddComponents)
				.Build()
				.RunAsync();

		/// <summary>
		/// Add all required components to <paramref name="services"/>. 
		/// </summary>
		private static void AddComponents(IServiceCollection services)
			=> services
				.AddComponent<HwInfoAppComponent>()
				.AddComponent<InfrastructureAppComponent>();
	}
}
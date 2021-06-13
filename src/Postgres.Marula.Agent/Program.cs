using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Postgres.Marula.HwInfo;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.Hosting;
using Postgres.Marula.WebApi.Common;

namespace Postgres.Marula.Agent
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
				.WithConfig(args, "marula-agent-config")
				.WebWithDefaultStartup()
				.ConfigureServices(services => services.AddComponent<HwInfoAppComponent>())
				.Build()
				.RunAsync();
	}
}
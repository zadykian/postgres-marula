using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Postgres.Marula.HttpApi.Common;
using Postgres.Marula.HwInfo;
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
				.ConfigureServices(services => services.AddComponent<HwInfoAppComponent>())
				.Build()
				.RunAsync();
	}
}
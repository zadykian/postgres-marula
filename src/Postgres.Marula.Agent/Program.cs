using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Postgres.Marula.HwInfo;
using Postgres.Marula.Infrastructure;
using Postgres.Marula.Infrastructure.Extensions;

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
				.ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
				.ConfigureServices(services => services.AddComponent<HwInfoAppComponent>())
				.Build()
				.RunAsync();
	}
}
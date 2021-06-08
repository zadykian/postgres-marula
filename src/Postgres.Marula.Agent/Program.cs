using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Postgres.Marula.HwInfo;
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
			=> Host
				.CreateDefaultBuilder(args)
				.AddJsonConfig("marula-agent-config")
				.UseDefaultServiceProvider(options =>
				{
					options.ValidateScopes = true;
					options.ValidateOnBuild = true;
				})
				.ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
				.ConfigureServices(services => services.AddComponent<HwInfoAppComponent>())
				.Build()
				.RunAsync();
	}
}
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
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
		private static void Main(string[] args)
			=> WebHost
				.CreateDefaultBuilder(args)
				.UseDefaultServiceProvider(options =>
				{
					options.ValidateScopes = true;
					options.ValidateOnBuild = true;
				})
				.ConfigureServices(services => services
					.AddComponent<HwInfoAppComponent>()
					.AddComponent<AgentAppComponent>())
				.Build()
				.Run();
	}
}
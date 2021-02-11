using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Postgres.Marula.Calculations;
using Postgres.Marula.DatabaseAccess;
using Postgres.Marula.Infrastructure;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.AppHost
{
	internal static class Program
	{
		/// <summary>
		/// Application entry point. 
		/// </summary>
		private static Task Main(string[] args)
			=> Host
				.CreateDefaultBuilder(args)
				.UseDefaultServiceProvider(options =>
				{
					options.ValidateScopes = true;
					options.ValidateOnBuild = true;
				})
				.ConfigureServices((_, services) => services
					.AddComponent<InfrastructureSolutionComponent>()
					.AddComponent<DatabaseAccessSolutionComponent>()
					.AddComponent<CalculationsSolutionComponent>()
					.AddComponent<AppHostSolutionComponent>())
				.Build()
				.RunAsync();
	}
}
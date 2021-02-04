using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.SolutionComponents;

namespace Postgres.Marula.AppHost
{
	internal static class Program
	{
		/// <summary>
		/// Application entry point. 
		/// </summary>
		private static async Task Main(string[] args)
			=> await Host
				.CreateDefaultBuilder(args)
				.ConfigureServices((_, services) => SolutionComponentsFactory
					.CreateAll()
					.ForEach(solutionComponent => solutionComponent.RegisterServices(services)))
				.Build()
				.RunAsync();
	}
}
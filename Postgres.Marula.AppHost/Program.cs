using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.SolutionComponents.Factory;

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
				.ConfigureServices((_, services) => new DefaultSolutionComponentsFactory()
					.To(factory => (ISolutionComponentsFactory) factory)
					.CreateAll()
					.ForEach(solutionComponent => solutionComponent.RegisterServices(services)))
				.Build()
				.RunAsync();
	}
}
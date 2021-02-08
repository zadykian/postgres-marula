using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.SolutionComponents.Factory;

namespace Postgres.Marula.AppHost
{
	/// <summary>
	/// Application.
	/// </summary>
	internal class Application
	{
		private readonly IHostBuilder hostBuilder;
		private readonly ISolutionComponentsFactory solutionComponentsFactory;

		public Application(
			IHostBuilder hostBuilder,
			ISolutionComponentsFactory solutionComponentsFactory)
		{
			this.hostBuilder = hostBuilder;
			this.solutionComponentsFactory = solutionComponentsFactory;
		}

		/// <summary>
		/// Run application. 
		/// </summary>
		public Task RunAsync()
			=> hostBuilder
				.ConfigureServices((_, services) => solutionComponentsFactory
					.CreateAll()
					.ForEach(solutionComponent => solutionComponent.RegisterServices(services)))
				.Build()
				.RunAsync();
	}
}
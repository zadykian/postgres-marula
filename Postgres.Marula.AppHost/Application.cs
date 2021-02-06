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
		private readonly ISolutionComponentsFactory solutionComponentsFactory;
		private readonly IHostBuilder hostBuilder;

		public Application(
			ISolutionComponentsFactory solutionComponentsFactory,
			IHostBuilder hostBuilder)
		{
			this.solutionComponentsFactory = solutionComponentsFactory;
			this.hostBuilder = hostBuilder;
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
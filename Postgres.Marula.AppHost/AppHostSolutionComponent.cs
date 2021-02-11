using Microsoft.Extensions.DependencyInjection;
using Postgres.Marula.Infrastructure.SolutionComponents;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.AppHost
{
	/// <inheritdoc />
	internal class AppHostSolutionComponent : ISolutionComponent
	{
		/// <inheritdoc />
		void ISolutionComponent.RegisterServices(IServiceCollection serviceCollection)
			=> serviceCollection.AddHostedService<HostedService>();
	}
}
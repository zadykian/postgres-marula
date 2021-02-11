using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Postgres.Marula.Infrastructure.Configuration;
using Postgres.Marula.Infrastructure.SolutionComponents;

[assembly: InternalsVisibleTo("Postgres.Marula.Tests")]

namespace Postgres.Marula.Infrastructure
{
	/// <inheritdoc />
	public class InfrastructureSolutionComponent : ISolutionComponent
	{
		/// <inheritdoc />
		void ISolutionComponent.RegisterServices(IServiceCollection serviceCollection)
			=> serviceCollection.AddSingleton<IAppConfiguration, DefaultAppConfiguration>();
	}
}
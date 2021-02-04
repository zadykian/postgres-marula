using Microsoft.Extensions.DependencyInjection;
using Postgres.Marula.DatabaseAccess.Conventions;
using Postgres.Marula.Infrastructure.SolutionComponents;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.DatabaseAccess
{
	/// <inheritdoc />
	internal class DatabaseAccessSolutionComponent : ISolutionComponent
	{
		/// <inheritdoc />
		void ISolutionComponent.RegisterServices(IServiceCollection serviceCollection)
			=> serviceCollection
				.AddSingleton<INamingConventions, DefaultNamingConventions>();
	}
}
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Postgres.Marula.DatabaseAccess.Conventions;
using Postgres.Marula.DatabaseAccess.SqlScripts.Provider;
using Postgres.Marula.Infrastructure.SolutionComponents;

// ReSharper disable UnusedType.Global

[assembly: InternalsVisibleTo("Postgres.Marula.Tests")]

namespace Postgres.Marula.DatabaseAccess
{
	/// <inheritdoc />
	internal class DatabaseAccessSolutionComponent : ISolutionComponent
	{
		/// <inheritdoc />
		void ISolutionComponent.RegisterServices(IServiceCollection serviceCollection)
			=> serviceCollection
				.AddSingleton<INamingConventions, DefaultNamingConventions>()
				.AddSingleton<ISqlScriptsProvider, AssemblyResourcesSqlScriptsProvider>();
	}
}
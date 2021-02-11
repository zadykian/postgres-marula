using System.Data;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Postgres.Marula.DatabaseAccess.Conventions;
using Postgres.Marula.DatabaseAccess.SqlScripts.Executor;
using Postgres.Marula.DatabaseAccess.SqlScripts.Provider;
using Postgres.Marula.Infrastructure.Configuration;
using Postgres.Marula.Infrastructure.Extensions;
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
				.AddSingleton<ISqlScriptsProvider, AssemblyResourcesSqlScriptsProvider>()
				.AddScoped<IDbConnection>(serviceProvider
					=> serviceProvider
						.GetRequiredService<IAppConfiguration>()
						.GetConnectionString()
						.To(connectionString => new NpgsqlConnection(connectionString))
						.Then(dbConnection => dbConnection.Open()))
				.AddSingleton<ISqlScriptsExecutor, DefaultSqlScriptsExecutor>();
	}
}
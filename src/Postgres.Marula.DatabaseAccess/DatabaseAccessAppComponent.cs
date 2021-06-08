using System;
using System.Data;
using System.Runtime.CompilerServices;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.DatabaseAccess.Configuration;
using Postgres.Marula.DatabaseAccess.ConnectionFactory;
using Postgres.Marula.DatabaseAccess.Conventions;
using Postgres.Marula.DatabaseAccess.DapperTypeHandlers;
using Postgres.Marula.DatabaseAccess.ServerInteraction;
using Postgres.Marula.DatabaseAccess.SqlScripts.Executor;
using Postgres.Marula.DatabaseAccess.SqlScripts.Provider;
using Postgres.Marula.Infrastructure.AppComponents;
using Postgres.Marula.Infrastructure.Extensions;

[assembly: InternalsVisibleTo("Postgres.Marula.Tests")]

namespace Postgres.Marula.DatabaseAccess
{
	/// <inheritdoc />
	public class DatabaseAccessAppComponent : IAppComponent
	{
		public DatabaseAccessAppComponent()
		{
			SqlMapper.AddTypeHandler(new NonEmptyStringTypeHandler());
			SqlMapper.AddTypeHandler(new DatabaseObjectNameTypeHandler());
			SqlMapper.AddTypeHandler(new LogSeqNumberTypeHandler());
			SqlMapper.AddTypeHandler(new FractionTypeHandler());
		}

		/// <inheritdoc />
		void IAppComponent.RegisterServices(IServiceCollection serviceCollection)
			=> serviceCollection
				.AddSingleton<INamingConventions, DefaultNamingConventions>()
				.AddSingleton<ISqlScriptsProvider, AssemblyResourcesSqlScriptsProvider>()
				.AddSingleton<ISqlScriptsExecutor, DefaultSqlScriptsExecutor>()
				.AddSingleton<IDatabaseAccessConfiguration, DatabaseAccessConfiguration>()
				.AddScoped(DbConnectionFactoryMethod)
				.AddScoped<IDbConnectionFactory, DefaultDbConnectionFactory>()
				.AddScoped<IDatabaseServer, DefaultDatabaseServer>()
				.AddScoped<ISystemStorage, DefaultSystemStorage>();

		/// <summary>
		/// Database connection factory method. 
		/// </summary>
		private static IDbConnection DbConnectionFactoryMethod(IServiceProvider serviceProvider)
			=> serviceProvider
				.GetRequiredService<IDatabaseAccessConfiguration>()
				.ConnectionString()
				.To(connectionString => new NpgsqlConnection(connectionString));
	}
}
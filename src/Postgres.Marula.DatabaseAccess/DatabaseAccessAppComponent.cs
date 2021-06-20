using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.PublicApi;
using Postgres.Marula.DatabaseAccess.Configuration;
using Postgres.Marula.DatabaseAccess.ConnectionFactory;
using Postgres.Marula.DatabaseAccess.Conventions;
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
		public DatabaseAccessAppComponent() => RegisterDapperTypeHandlers();

		/// <inheritdoc />
		IServiceCollection IAppComponent.RegisterServices(IServiceCollection serviceCollection)
			=> serviceCollection
				.AddSingleton<INamingConventions, DefaultNamingConventions>()
				.AddSingleton<ISqlScriptsProvider, AssemblyResourcesSqlScriptsProvider>()
				.AddSingleton<ISqlScriptsExecutor, DefaultSqlScriptsExecutor>()
				.AddSingleton<IDatabaseAccessConfiguration, DatabaseAccessConfiguration>()
				.AddScoped(DbConnectionFactoryMethod)
				.AddScoped<IDbConnectionFactory, DefaultDbConnectionFactory>()
				.AddScoped<IDatabaseServer, DefaultDatabaseServer>()
				.AddScoped<ISystemStorage, DefaultSystemStorage>()
				.Forward<ISystemStorage, IParameterValues>();

		/// <summary>
		/// Register all implementations of <see cref="SqlMapper.ITypeHandler"/> defined in current assembly.
		/// </summary>
		private static void RegisterDapperTypeHandlers()
			=> Assembly
				.GetExecutingAssembly()
				.GetTypes()
				.Where(type => !type.IsAbstract && type.IsAssignableTo(typeof(SqlMapper.ITypeHandler)))
				.Select(Activator.CreateInstance)
				.Cast<dynamic>()
				.ForEach(typeHandler => SqlMapper.AddTypeHandler(typeHandler));

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
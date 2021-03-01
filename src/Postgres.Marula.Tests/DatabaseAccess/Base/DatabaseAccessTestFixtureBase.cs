using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParameterValueParsing;
using Postgres.Marula.DatabaseAccess;
using Postgres.Marula.DatabaseAccess.Conventions;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;
using Postgres.Marula.Tests.Base;

namespace Postgres.Marula.Tests.DatabaseAccess.Base
{
	/// <summary>
	/// Base class for testing services from database access component.
	/// </summary>
	internal abstract class DatabaseAccessTestFixtureBase : SingleComponentTestFixtureBase<DatabaseAccessAppComponent>
	{
		/// <summary>
		/// Method that is called once.
		/// </summary>
		[OneTimeSetUp]
		public async Task OneTimeSetUp()
		{
			var commandText = $@"
				drop schema if exists {GetService<INamingConventions>().SystemSchemaName} cascade;
				reset all;
				select pg_reload_conf();";

			var dbConnection = GetService<IDbConnection>();
			dbConnection.Open();
			await dbConnection.ExecuteAsync(commandText);
		}

		/// <inheritdoc />
		protected override void ConfigureServices(IServiceCollection serviceCollection)
		{
			base.ConfigureServices(serviceCollection);
			serviceCollection
				.AddSingleton<INamingConventions, TestNamingConventions>()
				.AddSingleton<IParameterValueParser, DefaultParameterValueParser>()

				// parameters are required for dictionary table filling.
				.AddBasedOn<IParameter>(ServiceLifetime.Transient)
				.Forward<IParameter, IParameterLink>();
		}

		/// <inheritdoc />
		private sealed class TestNamingConventions : DefaultNamingConventions
		{
			/// <inheritdoc />
			public override DatabaseObjectName SystemSchemaName => "marula_tool_unit_tests";
		}
	}
}
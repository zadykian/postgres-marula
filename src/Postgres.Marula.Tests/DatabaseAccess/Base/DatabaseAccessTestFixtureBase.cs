using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Postgres.Marula.Calculations.ParameterValues.Parsing;
using Postgres.Marula.DatabaseAccess;
using Postgres.Marula.DatabaseAccess.Conventions;
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
			await dbConnection.ExecuteAsync(TestDdlScript());
		}

		/// <summary>
		/// DDL script to create test objects in separate schema.
		/// </summary>
		private static NonEmptyString TestDdlScript()
			=> @"
				drop schema if exists marula_tool_unit_tests_data cascade;
				create schema marula_tool_unit_tests_data;
				set search_path to marula_tool_unit_tests_data;

				create table parent_table
				(
					id int primary key generated always as identity,
					name text
				)
				partition by hash(id);

				create table partition_0
				partition of parent_table
				for values with (modulus 2, remainder 0);

				create table partition_1
				partition of parent_table
				for values with (modulus 2, remainder 1);";

		/// <inheritdoc />
		protected override void ConfigureServices(IServiceCollection serviceCollection)
		{
			base.ConfigureServices(serviceCollection);
			serviceCollection
				.AddSingleton<INamingConventions, TestNamingConventions>()
				.AddSingleton<IParameterValueParser, DefaultParameterValueParser>();
		}

		/// <inheritdoc />
		private sealed class TestNamingConventions : DefaultNamingConventions
		{
			/// <inheritdoc />
			public override DatabaseObjectName SystemSchemaName => "marula_tool_unit_tests";
		}
	}
}
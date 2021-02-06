using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Postgres.Marula.DatabaseAccess;
using Postgres.Marula.DatabaseAccess.Conventions;
using Postgres.Marula.DatabaseAccess.SqlScripts.Executor;
using Postgres.Marula.Tests.Base;

namespace Postgres.Marula.Tests.DatabaseAccess
{
	/// <summary>
	/// SQL scripts execution tests.
	/// </summary>
	internal class SqlScriptsExecutorTests : SingleComponentTestFixtureBase<DatabaseAccessSolutionComponent>
	{
		private INamingConventions namingConventions;

		/// <inheritdoc />
		public override void OneTimeSetUp()
		{
			base.OneTimeSetUp();
			namingConventions = GetService<INamingConventions>();
			using var dbConnection = GetService<IDbConnection>();
			dbConnection.Execute($"drop schema if exists {namingConventions.SystemSchemaName} cascade;");
		}

		/// <summary>
		/// Database connection creation with system schema initialization.
		/// </summary>
		[Test]
		public async Task ScriptExecutionTest()
		{
			var scriptsExecutor = GetService<ISqlScriptsExecutor>();
			using var dbConnection = GetService<IDbConnection>();

			await scriptsExecutor.ExecuteScriptsAsync(dbConnection);

			var systemSchemaExists = await dbConnection.QuerySingleAsync<bool>($@"
				select exists (
					select 1
					from pg_catalog.pg_namespace
					where nspname = @{nameof(INamingConventions.SystemSchemaName)});",
				new {namingConventions.SystemSchemaName});

			Assert.IsTrue(systemSchemaExists);
		}

		/// <inheritdoc />
		protected override void ConfigureServices(IServiceCollection serviceCollection)
		{
			base.ConfigureServices(serviceCollection);
			serviceCollection.AddSingleton<INamingConventions, TestNamingConventions>();
		}

		/// <inheritdoc />
		private sealed class TestNamingConventions : INamingConventions
		{
			/// <inheritdoc />
			string INamingConventions.SystemSchemaName => "marula_tool_unit_tests";
		}
	}
}
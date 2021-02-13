using System.Data;
using System.Threading.Tasks;
using Dapper;
using NUnit.Framework;
using Postgres.Marula.DatabaseAccess.Conventions;
using Postgres.Marula.DatabaseAccess.SqlScripts.Executor;
using Postgres.Marula.Tests.DatabaseAccess.Base;

namespace Postgres.Marula.Tests.DatabaseAccess
{
	/// <summary>
	/// SQL scripts execution tests.
	/// </summary>
	internal class SqlScriptsExecutorTests : DatabaseAccessTestFixtureBase
	{
		/// <summary>
		/// Database system schema initialization test.
		/// </summary>
		[Test]
		public async Task ScriptExecutionTest()
		{
			var scriptsExecutor = GetService<ISqlScriptsExecutor>();
			var namingConventions = GetService<INamingConventions>();
			var dbConnection = GetService<IDbConnection>();

			await scriptsExecutor.ExecuteScriptsAsync(dbConnection);

			var systemSchemaExists = await dbConnection.QuerySingleAsync<bool>($@"
				select exists (
					select null
					from pg_catalog.pg_namespace
					where nspname = @{nameof(INamingConventions.SystemSchemaName)});",
				new {namingConventions.SystemSchemaName});

			Assert.IsTrue(systemSchemaExists);
		}
	}
}
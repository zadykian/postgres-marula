using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Postgres.Marula.DatabaseAccess;
using Postgres.Marula.DatabaseAccess.ConnectionFactory;
using Postgres.Marula.DatabaseAccess.Conventions;
using Postgres.Marula.Tests.Base;

namespace Postgres.Marula.Tests.DatabaseAccess
{
	internal class DbConnectionFactoryTests
		: SingleServiceTestBase<IDbConnectionFactory, DatabaseAccessSolutionComponent>
	{
		[Test]
		public async Task ConnectionCreationTest()
		{
			var dbConnection = await ServiceToTest.GetPreparedConnectionAsync();
			try
			{
				await dbConnection.ExecuteAsync("select 1;");
			}
			finally
			{
				await ServiceToTest.ReleaseConnectionAsync(dbConnection);
			}
		}

		protected override void ConfigureServices(IServiceCollection serviceCollection)
		{
			base.ConfigureServices(serviceCollection);
			serviceCollection.AddSingleton<INamingConventions, TestNamingConventions>();
		}

		private sealed class TestNamingConventions : INamingConventions
		{
			/// <inheritdoc />
			string INamingConventions.SystemSchemaName => "pg_marula_unit_tests";
		}
	}
}
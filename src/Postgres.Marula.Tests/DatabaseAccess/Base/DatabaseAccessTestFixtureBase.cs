using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Postgres.Marula.DatabaseAccess;
using Postgres.Marula.DatabaseAccess.Conventions;
using Postgres.Marula.Infrastructure.TypeDecorators;
using Postgres.Marula.Tests.Base;

namespace Postgres.Marula.Tests.DatabaseAccess.Base
{
	/// <summary>
	/// Base class for testing services from database access component.
	/// </summary>
	internal abstract class DatabaseAccessTestFixtureBase : SingleComponentTestFixtureBase<DatabaseAccessSolutionComponent>
	{
		/// <summary>
		/// Method that is called once.
		/// </summary>
		[OneTimeSetUp]
		public async Task OneTimeSetUp()
		{
			var namingConventions = GetService<INamingConventions>();
			var dbConnection = GetService<IDbConnection>();
			dbConnection.Open();
			await dbConnection.ExecuteAsync($"drop schema if exists {namingConventions.SystemSchemaName} cascade;");
		}

		/// <inheritdoc />
		protected override void ConfigureServices(IServiceCollection serviceCollection)
		{
			base.ConfigureServices(serviceCollection);
			serviceCollection.AddSingleton<INamingConventions, TestNamingConventions>();
		}

		/// <inheritdoc />
		private sealed class TestNamingConventions : DefaultNamingConventions
		{
			/// <inheritdoc />
			public override DatabaseObjectName SystemSchemaName => "marula_tool_unit_tests";
		}
	}
}
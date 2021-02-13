using Microsoft.Extensions.DependencyInjection;
using Postgres.Marula.DatabaseAccess;
using Postgres.Marula.DatabaseAccess.Conventions;
using Postgres.Marula.Infrastructure.TypeDecorators;
using Postgres.Marula.Tests.Base;

namespace Postgres.Marula.Tests.DatabaseAccess.Base
{
	/// <summary>
	/// Base class for testing services from database access component.
	/// </summary>
	internal abstract class DatabaseAccessTestFixtureBase  : SingleComponentTestFixtureBase<DatabaseAccessSolutionComponent>
	{
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
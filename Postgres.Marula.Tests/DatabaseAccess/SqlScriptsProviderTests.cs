using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Postgres.Marula.DatabaseAccess;
using Postgres.Marula.DatabaseAccess.SqlScripts.Provider;
using Postgres.Marula.Infrastructure.SolutionComponents;

namespace Postgres.Marula.Tests.DatabaseAccess
{
	[TestFixture]
	public class SqlScriptsProviderTests
	{
		private ISqlScriptsProvider sqlScriptsProvider;

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			var serviceCollection = new ServiceCollection();
			var databaseAccessComponent = new DatabaseAccessSolutionComponent() as ISolutionComponent;
			databaseAccessComponent.RegisterServices(serviceCollection);

			sqlScriptsProvider = serviceCollection
				.BuildServiceProvider()
				.GetRequiredService<ISqlScriptsProvider>();
		}

		[Test]
		public void GetAllScriptsTest()
		{
			var sqlScripts = sqlScriptsProvider.GetAllOrderedByExecution();
			Assert.IsTrue(sqlScripts.Any());
		}
	}
}
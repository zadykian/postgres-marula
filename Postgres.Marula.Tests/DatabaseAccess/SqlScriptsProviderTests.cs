using System.Linq;
using NUnit.Framework;
using Postgres.Marula.DatabaseAccess;
using Postgres.Marula.DatabaseAccess.SqlScripts.Provider;
using Postgres.Marula.Tests.Base;

namespace Postgres.Marula.Tests.DatabaseAccess
{
	internal class SqlScriptsProviderTests : SingleComponentTestFixtureBase<DatabaseAccessSolutionComponent>
	{
		[Test]
		public void GetAllScriptsTest()
		{
			var sqlScripts = GetService<ISqlScriptsProvider>().GetAllOrderedByExecution();
			Assert.IsTrue(sqlScripts.Any());
		}
	}
}
using System.Linq;
using NUnit.Framework;
using Postgres.Marula.DatabaseAccess;
using Postgres.Marula.DatabaseAccess.SqlScripts.Provider;
using Postgres.Marula.Tests.Base;

namespace Postgres.Marula.Tests.DatabaseAccess
{
	internal class SqlScriptsProviderTests
		: SingleServiceTestBase<ISqlScriptsProvider, DatabaseAccessSolutionComponent>
	{
		[Test]
		public void GetAllScriptsTest()
		{
			var sqlScripts = ServiceToTest.GetAllOrderedByExecution();
			Assert.IsTrue(sqlScripts.Any());
		}
	}
}
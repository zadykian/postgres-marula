using System.Linq;
using NUnit.Framework;
using Postgres.Marula.DatabaseAccess;
using Postgres.Marula.DatabaseAccess.SqlScripts.Provider;
using Postgres.Marula.Tests.Base;

namespace Postgres.Marula.Tests.DatabaseAccess
{
	/// <summary>
	/// SQL scripts provider tests.
	/// </summary>
	internal class SqlScriptsProviderTests : SingleComponentTestFixtureBase<DatabaseAccessSolutionComponent>
	{
		/// <summary>
		/// Get all required scripts via provider.
		/// </summary>
		[Test]
		public void GetAllScriptsTest()
		{
			var sqlScripts = GetService<ISqlScriptsProvider>().GetAllOrderedByExecution();
			Assert.IsTrue(sqlScripts.Any());
		}
	}
}
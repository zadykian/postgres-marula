using System.Linq;
using NUnit.Framework;
using Postgres.Marula.DatabaseAccess.SqlScripts.Provider;
using Postgres.Marula.Tests.DatabaseAccess.Base;

namespace Postgres.Marula.Tests.DatabaseAccess
{
	/// <summary>
	/// SQL scripts provider tests.
	/// </summary>
	internal class SqlScriptsProviderTests : DatabaseAccessTestFixtureBase
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
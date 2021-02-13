using System.Data;
using System.Threading.Tasks;
using NUnit.Framework;
using Postgres.Marula.DatabaseAccess;
using Postgres.Marula.DatabaseAccess.ConnectionFactory;
using Postgres.Marula.Tests.Base;

namespace Postgres.Marula.Tests.DatabaseAccess
{
	/// <summary>
	/// Database connection factory tests.
	/// </summary>
	internal class ConnectionFactoryTests : SingleComponentTestFixtureBase<DatabaseAccessSolutionComponent>
	{
		/// <summary>
		/// Prepared connection creation test. 
		/// </summary>
		[Test]
		public async Task PreparedConnectionCreationTest()
		{
			var connectionFactory = GetService<IPreparedDbConnectionFactory>();

			var firstPreparedConnection = await connectionFactory.GetPreparedConnectionAsync();
			Assert.AreEqual(ConnectionState.Open, firstPreparedConnection.State);

			var secondPreparedConnection = await connectionFactory.GetPreparedConnectionAsync();
			Assert.AreSame(firstPreparedConnection, secondPreparedConnection);
		}
	}
}
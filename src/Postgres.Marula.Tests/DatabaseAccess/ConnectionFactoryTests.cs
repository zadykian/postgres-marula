using System.Data;
using System.Threading.Tasks;
using NUnit.Framework;
using Postgres.Marula.DatabaseAccess.ConnectionFactory;
using Postgres.Marula.Tests.DatabaseAccess.Base;

namespace Postgres.Marula.Tests.DatabaseAccess
{
	/// <summary>
	/// Database connection factory tests.
	/// </summary>
	internal class ConnectionFactoryTests : DatabaseAccessTestFixtureBase
	{
		/// <summary>
		/// Prepared connection creation test.
		/// </summary>
		[Test]
		public async Task PreparedConnectionCreationTest()
		{
			var connectionFactory = GetService<IDbConnectionFactory>();

			var firstPreparedConnection = await connectionFactory.GetConnectionAsync();
			Assert.AreEqual(ConnectionState.Open, firstPreparedConnection.State);

			var secondPreparedConnection = await connectionFactory.GetConnectionAsync();
			Assert.AreSame(firstPreparedConnection, secondPreparedConnection);
		}
	}
}
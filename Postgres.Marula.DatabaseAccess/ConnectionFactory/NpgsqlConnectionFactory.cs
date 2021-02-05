using System.Data;

namespace Postgres.Marula.DatabaseAccess.ConnectionFactory
{
	/// <inheritdoc />
	internal class NpgsqlConnectionFactory : IDbConnectionFactory
	{
		/// <inheritdoc />
		IDbConnection IDbConnectionFactory.GetPreparedConnection()
		{
			var dbConnection = new NpgsqlConnection();
		}

		/// <inheritdoc />
		void IDbConnectionFactory.ReleaseConnection(IDbConnection dbConnection) => dbConnection.Dispose();
	}
}
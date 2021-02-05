using System.Data;

namespace Postgres.Marula.DatabaseAccess.ConnectionFactory
{
	/// <summary>
	/// Database connection factory.
	/// </summary>
	internal interface IDbConnectionFactory
	{
		/// <summary>
		/// Get prepared database connection.
		/// </summary>
		/// <remarks>
		/// Preparation implies execution of all required SQL scripts.
		/// </remarks>
		IDbConnection GetPreparedConnection();

		/// <summary>
		/// Release database connection. 
		/// </summary>
		void ReleaseConnection(IDbConnection dbConnection);
	}
}
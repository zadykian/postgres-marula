using System.Data;
using System.Threading.Tasks;

namespace Postgres.Marula.DatabaseAccess.ConnectionFactory
{
	/// <summary>
	/// Database connection factory.
	/// </summary>
	internal interface IDbConnectionFactory
	{
		/// <summary>
		/// Get connection which is prepared for future communications with server.
		/// </summary>
		Task<IDbConnection> GetConnectionAsync();
	}
}
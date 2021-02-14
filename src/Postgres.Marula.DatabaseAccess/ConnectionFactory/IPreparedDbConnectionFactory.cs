using System.Data;
using System.Threading.Tasks;

namespace Postgres.Marula.DatabaseAccess.ConnectionFactory
{
	internal interface IPreparedDbConnectionFactory
	{
		/// <summary>
		/// Get connection which is prepared for future communications with server.
		/// </summary>
		Task<IDbConnection> GetPreparedConnectionAsync();
	}
}
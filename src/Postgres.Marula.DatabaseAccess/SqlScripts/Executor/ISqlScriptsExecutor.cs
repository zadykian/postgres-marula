using System.Data;
using System.Threading.Tasks;

namespace Postgres.Marula.DatabaseAccess.SqlScripts.Executor
{
	/// <summary>
	/// SQL scripts executor.
	/// </summary>
	internal interface ISqlScriptsExecutor
	{
		/// <summary>
		/// Execute all required scripts within connection <paramref name="dbConnection"/>. 
		/// </summary>
		Task ExecuteScriptsAsync(IDbConnection dbConnection);
	}
}
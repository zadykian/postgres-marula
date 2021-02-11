using System.Collections.Generic;

namespace Postgres.Marula.DatabaseAccess.SqlScripts.Provider
{
	/// <summary>
	/// Provider of SQL scripts for system data definition and modification.
	/// </summary>
	internal interface ISqlScriptsProvider
	{
		/// <summary>
		/// Get all scripts sorted by execution order. 
		/// </summary>
		IEnumerable<SqlScript> GetAllOrderedByExecution();
	}
}
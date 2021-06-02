using System.Collections.Generic;
using Postgres.Marula.Calculations.ExternalDependencies;

namespace Postgres.Marula.Calculations.Parameters.Wal.LsnHistory
{
	/// <summary>
	/// Write-Ahead Log LSN history.
	/// </summary>
	internal interface IWalLsnHistory
	{
		/// <summary>
		/// Get Write-Ahead Log LSN history entries.
		/// </summary>
		IAsyncEnumerable<LsnHistoryEntry> ReadAsync();
	}
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.ExternalDependencies
{
	/// <summary>
	/// Application system storage.
	/// </summary>
	public interface ISystemStorage
	{
		/// <summary>
		/// Save calculated parameter values.
		/// </summary>
		Task SaveParameterValuesAsync(IReadOnlyCollection<ParameterValueWithStatus> parameterValues);

		/// <summary>
		/// Save current LSN value.
		/// </summary>
		Task SaveLogSeqNumberAsync(LogSeqNumber logSeqNumber);

		/// <summary>
		/// Get most resent LSN values which were saved not earlier
		/// then current time minus <paramref name="window"/>. 
		/// </summary>
		IAsyncEnumerable<LsnHistoryEntry> GetLsnHistory(PositiveTimeSpan window);
	}

	/// <summary>
	/// WAL insert location history single entry.
	/// </summary>
	public readonly struct LsnHistoryEntry
	{
		public LsnHistoryEntry(DateTime logTimestamp, LogSeqNumber walInsertLocation)
		{
			LogTimestamp = logTimestamp;
			WalInsertLocation = walInsertLocation;
		}

		/// <summary>
		/// LSN log timestamp.
		/// </summary>
		public DateTime LogTimestamp { get; }

		/// <summary>
		/// WAL insert location.
		/// </summary>
		public LogSeqNumber WalInsertLocation { get; }
	}
}
using System;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.ExternalDependencies
{
	/// <summary>
	/// WAL insert location history single entry.
	/// </summary>
	public readonly struct LsnHistoryEntry
	{
		// ReSharper disable once UnusedMember.Local
		private LsnHistoryEntry(DateTime logTimestamp, LogSeqNumber walInsertLocation)
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
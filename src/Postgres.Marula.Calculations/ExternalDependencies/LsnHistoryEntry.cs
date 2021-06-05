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

		/// <summary>
		/// Calculate WAL traffic per second between two log entries. 
		/// </summary>
		public Memory TrafficPerSecondBefore(LsnHistoryEntry entryInFuture)
		{
			if (LogTimestamp > entryInFuture.LogTimestamp)
			{
				throw new ArgumentException(
					$"'{nameof(entryInFuture)}' log entry must be captured after current entry.", nameof(entryInFuture));
			}

			var memory = entryInFuture.WalInsertLocation - WalInsertLocation;
			var timeSpan = entryInFuture.LogTimestamp - LogTimestamp;
			return new(memory.TotalBytes / (ulong) timeSpan.TotalSeconds);
		}
	}
}
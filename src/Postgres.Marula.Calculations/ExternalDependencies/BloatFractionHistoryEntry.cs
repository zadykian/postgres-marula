using System;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.ExternalDependencies
{
	/// <summary>
	/// Bloat fraction history single entry.
	/// </summary>
	public readonly struct BloatFractionHistoryEntry
	{
		public BloatFractionHistoryEntry(DateTime logTimestamp, Fraction averageBloatFraction)
		{
			LogTimestamp = logTimestamp;
			AverageBloatFraction = averageBloatFraction;
		}

		/// <summary>
		/// Bloat fraction log timestamp.
		/// </summary>
		public DateTime LogTimestamp { get; }

		/// <summary>
		/// Value of average bloat fraction.
		/// </summary>
		public Fraction AverageBloatFraction { get; }

		/// <inheritdoc />
		public override string ToString() => $"{LogTimestamp:s} -> {AverageBloatFraction}";
	}
}
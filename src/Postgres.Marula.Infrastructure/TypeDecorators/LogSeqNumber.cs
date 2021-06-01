using System;
using System.Globalization;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.Infrastructure.TypeDecorators
{
	/// <summary>
	/// 'pg_catalog.pg_lsn' data type object representation.
	/// </summary>
	/// <remarks>
	/// Write-Ahead Log insert location.
	/// </remarks>
	public readonly struct LogSeqNumber : IComparable<LogSeqNumber>, IComparable
	{
		private readonly uint major;
		private readonly uint minor;

		/// <param name="stringRepresentation">
		/// LSN string representation: '{major}/{minor}'.
		/// </param>
		public LogSeqNumber(NonEmptyString stringRepresentation)
		{
			var (majorStr, minorStr) = stringRepresentation
				.To(str => (string) str)
				.Split('/')
				.To(tokens => (tokens[0], tokens[1]));

			major = uint.Parse(majorStr, NumberStyles.HexNumber);
			minor = uint.Parse(minorStr, NumberStyles.HexNumber);
		}

		/// <inheritdoc />
		public override string ToString() => $"{major:x8}/{minor:x8}";

		/// <inheritdoc />
		public int CompareTo(LogSeqNumber other)
		{
			var majorComparison = major.CompareTo(other.major);
			return majorComparison != 0 ? majorComparison : minor.CompareTo(other.minor);
		}

		/// <inheritdoc />
		public int CompareTo(object? obj)
		{
			if (ReferenceEquals(null, obj)) return 1;
			return obj is LogSeqNumber other
				? CompareTo(other)
				: throw new ArgumentException($"Object must be of type {nameof(LogSeqNumber)}");
		}

		/// <summary>
		/// Operator which calculates the difference in bytes
		/// between two LSN values. 
		/// </summary>
		public static Memory operator -(LogSeqNumber left, LogSeqNumber right)
		{
			var majorDiff = (ulong) (left.major - right.major) << 32;
			var minorDiff = left.minor - right.minor;
			return new(majorDiff + minorDiff);
		}
	}
}
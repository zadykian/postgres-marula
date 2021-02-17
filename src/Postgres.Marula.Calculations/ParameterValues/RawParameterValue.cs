using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.ParameterValues
{
	/// <summary>
	/// Parameter value retrieved from database server without any processing.
	/// </summary>
	public readonly struct RawParameterValue
	{
		public RawParameterValue(NonEmptyString value, Range<decimal>? validRange)
		{
			Value = value;
			ValidRange = validRange;
		}

		/// <summary>
		/// Value string representation.
		/// </summary>
		public NonEmptyString Value { get; }

		/// <summary>
		/// Range of valid values.
		/// </summary>
		public Range<decimal>? ValidRange { get; }

		/// <inheritdoc />
		public override string ToString() => $"{Value} {ValidRange}";
	}
}
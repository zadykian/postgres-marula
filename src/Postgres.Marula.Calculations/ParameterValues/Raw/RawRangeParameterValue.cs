using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.ParameterValues.Raw
{
	/// <inheritdoc />
	public class RawRangeParameterValue : RawParameterValue
	{
		public RawRangeParameterValue(NonEmptyString value, Range<decimal> validRange) : base(value)
			=> ValidRange = validRange;

		/// <summary>
		/// Range of valid values.
		/// </summary>
		public Range<decimal> ValidRange { get; }

		/// <inheritdoc />
		public override string ToString() => $"'{base.ToString()}' with range: {ValidRange.ToString()}";
	}
}
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.ParameterValues.Raw
{
	/// <summary>
	/// Parameter value retrieved from database server without any processing.
	/// </summary>
	public class RawParameterValue
	{
		public RawParameterValue(NonEmptyString value, RawValueType type)
		{
			Value = value;
			Type = type;
		}

		/// <summary>
		/// Value string representation.
		/// </summary>
		public NonEmptyString Value { get; }

		public RawValueType Type { get; }

		/// <inheritdoc />
		public override string ToString() => Value;
	}
}
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.Parameters.Properties;
using Postgres.Marula.Calculations.Parameters.Values.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.Parameters.Values
{
	/// <summary>
	/// Database positive timespan parameter value.
	/// </summary>
	public class TimeSpanParameterValue : ParameterValueBase<PositiveTimeSpan>
	{
		public TimeSpanParameterValue(IParameterLink parameterLink, PositiveTimeSpan value)
			: base(parameterLink, value)
		{
		}

		/// <inheritdoc />
		public override ParameterUnit Unit => ParameterUnit.Milliseconds;

		/// <inheritdoc />
		public override string AsStringValue() => $"{Value.TotalMilliseconds}{Unit.AsString()}";
	}
}
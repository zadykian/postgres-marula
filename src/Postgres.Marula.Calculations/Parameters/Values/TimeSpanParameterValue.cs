using System;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.Parameters.Properties;
using Postgres.Marula.Calculations.Parameters.Values.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.Parameters.Values
{
	/// <summary>
	/// Database positive timespan parameter value.
	/// </summary>
	public class TimeSpanParameterValue : ParameterValueBase<PositiveTimespan>
	{
		public TimeSpanParameterValue(IParameterLink parameterLink, PositiveTimespan value)
			: base(parameterLink, value)
		{
		}

		public TimeSpanParameterValue(IParameterLink parameterLink, ulong milliseconds)
			: base(parameterLink, TimeSpan.FromMilliseconds(milliseconds))
		{
		}

		/// <inheritdoc />
		public override ParameterUnit Unit => ParameterUnit.Milliseconds;

		/// <inheritdoc />
		public override string AsStringValue() => $"{Value.TotalMilliseconds}{Unit.AsString()}";
	}
}
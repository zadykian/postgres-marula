using Postgres.Marula.Calculations.ParameterProperties;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.ParameterValues
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
		public override IUnit Unit => new IUnit.TimeSpan(Value.Normalized().Unit);
	}
}
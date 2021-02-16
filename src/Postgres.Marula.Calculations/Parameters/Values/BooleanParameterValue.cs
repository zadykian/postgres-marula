using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.Parameters.Properties;
using Postgres.Marula.Calculations.Parameters.Values.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.Parameters.Values
{
	/// <summary>
	/// Database boolean parameter value.
	/// </summary>
	public class BooleanParameterValue : ParameterValueBase<bool>
	{
		public BooleanParameterValue(IParameterLink parameterLink, bool value)
			: base(parameterLink, value)
		{
		}

		/// <inheritdoc />
		public override ParameterUnit Unit => ParameterUnit.None;

		/// <inheritdoc />
		public override NonEmptyString AsString() => Value.ToString().ToLower();
	}
}
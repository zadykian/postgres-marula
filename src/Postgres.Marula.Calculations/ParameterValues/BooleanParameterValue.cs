using Postgres.Marula.Calculations.ParameterProperties;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParameterValues.Base;

namespace Postgres.Marula.Calculations.ParameterValues
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
		public override IUnit Unit => new IUnit.None();
	}
}
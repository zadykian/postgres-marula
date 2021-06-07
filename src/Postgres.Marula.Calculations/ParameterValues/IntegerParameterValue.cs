using Postgres.Marula.Calculations.ParameterProperties;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParameterValues.Base;

namespace Postgres.Marula.Calculations.ParameterValues
{
	/// <summary>
	/// Database integer parameter value.
	/// </summary>
	public class IntegerParameterValue : ParameterValueBase<uint>
	{
		internal IntegerParameterValue(IParameterLink parameterLink, uint value)
			: base(parameterLink, value)
		{
		}

		/// <inheritdoc />
		public override ParameterUnit Unit => ParameterUnit.None;
	}
}
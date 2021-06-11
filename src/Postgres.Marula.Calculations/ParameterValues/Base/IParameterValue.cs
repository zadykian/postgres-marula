using Postgres.Marula.Calculations.ParameterProperties;
using Postgres.Marula.Calculations.Parameters.Base;

namespace Postgres.Marula.Calculations.ParameterValues.Base
{
	/// <summary>
	/// Calculated parameter value.
	/// </summary>
	public interface IParameterValue
	{
		/// <summary>
		/// Link to parameter.
		/// </summary>
		IParameterLink ParameterLink { get; }

		/// <summary>
		/// Parameter unit.
		/// </summary>
		IUnit Unit { get; }
	}
}
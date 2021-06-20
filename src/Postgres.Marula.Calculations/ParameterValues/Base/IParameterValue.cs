using Postgres.Marula.Calculations.ParameterProperties;

namespace Postgres.Marula.Calculations.ParameterValues.Base
{
	/// <summary>
	/// Calculated parameter value.
	/// </summary>
	public interface IParameterValue : IHasLinkToParameter
	{
		/// <summary>
		/// Parameter unit.
		/// </summary>
		IUnit Unit { get; }
	}
}
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.Parameters.Properties;

namespace Postgres.Marula.Calculations.Parameters.Values
{
	/// <summary>
	/// Calculated parameter value.
	/// </summary>
	internal interface IParameterValue
	{
		/// <summary>
		/// Link to parameter.
		/// </summary>
		IParameterLink ParameterLink { get; }

		/// <summary>
		/// Parameter unit.
		/// </summary>
		ParameterUnit Unit { get; }

		/// <summary>
		/// Represent parameter value as string to be applied to database server. 
		/// </summary>
		string AsStringValue();
	}
}
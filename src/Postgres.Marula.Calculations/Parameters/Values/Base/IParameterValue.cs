using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.Parameters.Properties;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.Parameters.Values.Base
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
		ParameterUnit Unit { get; }

		/// <summary>
		/// Represent parameter value as string without unit. 
		/// </summary>
		NonEmptyString AsString();
	}
}
namespace Postgres.Marula.Calculations.Parameters.Base
{
	/// <summary>
	/// Calculated parameter value.
	/// </summary>
	internal interface IParameterValue
	{
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
using Postgres.Marula.Calculations.Parameters.Properties;

namespace Postgres.Marula.Calculations.Parameters.Values.Base
{
	/// <summary>
	/// Database parameter value with calculation status.
	/// </summary>
	public readonly struct ParameterValueWithStatus
	{
		public ParameterValueWithStatus(IParameterValue value, CalculationStatus calculationStatus)
		{
			Value = value;
			CalculationStatus = calculationStatus;
		}

		/// <summary>
		/// Calculated parameter value.
		/// </summary>
		public IParameterValue Value { get; }

		/// <summary>
		/// Parameter calculation status.
		/// </summary>
		public CalculationStatus CalculationStatus { get; }
	}
}
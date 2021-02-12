using Postgres.Marula.Calculations.Parameters.Properties;

namespace Postgres.Marula.Calculations.Parameters.Values
{
	/// <summary>
	/// Database parameter value with calculation status.
	/// </summary>
	public readonly struct ParameterValueWithStatus
	{
		public ParameterValueWithStatus(IParameterValue value, ParameterStatus parameterStatus)
		{
			Value = value;
			ParameterStatus = parameterStatus;
		}

		/// <summary>
		/// Calculated parameter value.
		/// </summary>
		public IParameterValue Value { get; }

		/// <summary>
		/// Parameter calculation status.
		/// </summary>
		public ParameterStatus ParameterStatus { get; }
	}
}
using System;
using System.Runtime.Serialization;

namespace Postgres.Marula.Calculations.Parameters.Exceptions
{
	/// <summary>
	/// Error related to parameters calculations.
	/// </summary>
	[Serializable]
	public class ParameterValueCalculationException : ApplicationException
	{
		/// <inheritdoc />
		public ParameterValueCalculationException(string message) : base(message)
		{
		}

		/// <inheritdoc />
		protected ParameterValueCalculationException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}
}
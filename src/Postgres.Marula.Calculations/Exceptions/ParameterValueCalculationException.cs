using System;
using System.Runtime.Serialization;

namespace Postgres.Marula.Calculations.Exceptions
{
	/// <summary>
	/// Error related to parameters calculations.
	/// </summary>
	[Serializable]
	internal class ParameterValueCalculationException : ApplicationException
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
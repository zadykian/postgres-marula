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
		/// <summary>
		/// Initializes a new instance of the <see cref="RemoteAgentAccessException"/> class
		/// with a specified error message.
		/// </summary>
		public ParameterValueCalculationException(string message) : base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ParameterValueCalculationException"/> class
		/// with serialized data.
		/// </summary>
		protected ParameterValueCalculationException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}
}
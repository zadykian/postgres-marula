using System;
using System.Runtime.Serialization;

namespace Postgres.Marula.Calculations.Exceptions
{
	/// <summary>
	/// Exception being raised on remote access failure.
	/// </summary>
	[Serializable]
	public class RemoteAgentAccessException : ApplicationException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RemoteAgentAccessException"/> class
		/// with a specified error message.
		/// </summary>
		public RemoteAgentAccessException(string message, Exception exception) : base(message, exception)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RemoteAgentAccessException"/> class
		/// with serialized data.
		/// </summary>
		protected RemoteAgentAccessException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}
}
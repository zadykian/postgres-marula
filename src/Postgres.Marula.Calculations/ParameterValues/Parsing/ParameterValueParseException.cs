using System;
using System.Runtime.Serialization;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.ParameterValues.Parsing
{
	/// <summary>
	/// Exception related to database parameter value parsing.
	/// </summary>
	[Serializable]
	internal class ParameterValueParseException : ApplicationException
	{
		/// <inheritdoc />
		public ParameterValueParseException(NonEmptyString message) : base(message)
		{
		}

		/// <inheritdoc />
		protected ParameterValueParseException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}
}
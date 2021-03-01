using System;
using System.Runtime.Serialization;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.DatabaseAccess.ServerInteraction.Exceptions
{
	// todo

	/// <summary>
	/// Exception related to system storage access.
	/// </summary>
	public class SystemStorageAccessException : ApplicationException
	{
		/// <inheritdoc />
		public SystemStorageAccessException(NonEmptyString message) : base(message)
		{
		}

		/// <inheritdoc />
		protected SystemStorageAccessException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}
}
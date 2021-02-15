using System;
using System.Runtime.Serialization;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.DatabaseAccess.ServerInteraction.Exceptions
{
	/// <summary>
	/// Exception related to management of database server configuration.
	/// </summary>
	[Serializable]
	internal class DatabaseServerConfigurationException : ApplicationException
	{
		/// <inheritdoc />
		public DatabaseServerConfigurationException(NonEmptyString message) : base(message)
		{
		}

		/// <inheritdoc />
		protected DatabaseServerConfigurationException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}
}
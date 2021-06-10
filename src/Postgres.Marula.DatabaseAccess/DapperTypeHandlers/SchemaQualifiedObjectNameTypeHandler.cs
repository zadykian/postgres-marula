using Postgres.Marula.DatabaseAccess.DapperTypeHandlers.Base;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.DatabaseAccess.DapperTypeHandlers
{
	/// <summary>
	/// <see cref="SchemaQualifiedObjectName"/> dapper type handler.
	/// </summary>
	internal class SchemaQualifiedObjectNameTypeHandler : StringLikeTypeHandlerBase<SchemaQualifiedObjectName>
	{
		/// <inheritdoc />
		public override SchemaQualifiedObjectName Parse(object value)
			=> value
				.ToString()
				.ThrowIfNull()
				.To(SchemaQualifiedObjectName.Parse);
	}
}
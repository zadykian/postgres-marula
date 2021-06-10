using System.Data;
using Dapper;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.DatabaseAccess.DapperTypeHandlers
{
	/// <summary>
	/// <see cref="SchemaQualifiedObjectName"/> dapper type handler.
	/// </summary>
	internal class SchemaQualifiedObjectNameTypeHandler : SqlMapper.TypeHandler<SchemaQualifiedObjectName>
	{
		/// <inheritdoc />
		public override void SetValue(IDbDataParameter parameter, SchemaQualifiedObjectName value)
		{
			parameter.DbType = DbType.String;
			parameter.Value = value.ToString();
		}

		/// <inheritdoc />
		public override SchemaQualifiedObjectName Parse(object value)
			=> value
				.ToString()
				.ThrowIfNull()
				.To(SchemaQualifiedObjectName.Parse);
	}
}
using System;
using System.Data;
using Dapper;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.DatabaseAccess.DapperTypeHandlers
{
	/// <summary>
	/// <see cref="DatabaseObjectName"/> dapper type handler.
	/// </summary>
	internal class DatabaseObjectNameTypeHandler : SqlMapper.TypeHandler<DatabaseObjectName>
	{
		/// <inheritdoc />
		public override void SetValue(IDbDataParameter parameter, DatabaseObjectName value)
		{
			parameter.DbType = DbType.String;
			parameter.Value = value.ToString();
		}

		/// <inheritdoc />
		public override DatabaseObjectName Parse(object value)
			=> value.ToString() ?? throw new ArgumentNullException(nameof(value));
	}
}
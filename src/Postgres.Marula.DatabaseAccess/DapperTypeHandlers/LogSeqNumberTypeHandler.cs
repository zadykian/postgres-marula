using System;
using System.Data;
using Dapper;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.DatabaseAccess.DapperTypeHandlers
{
	/// <summary>
	/// <see cref="LogSeqNumber"/> dapper type handler.
	/// </summary>
	internal class LogSeqNumberTypeHandler : SqlMapper.TypeHandler<LogSeqNumber>
	{
		/// <inheritdoc />
		public override void SetValue(IDbDataParameter parameter, LogSeqNumber value)
		{
			parameter.DbType = DbType.String;
			parameter.Value = value.ToString();
		}

		/// <inheritdoc />
		public override LogSeqNumber Parse(object value)
		{
			var stringValue = value.ToString() ?? throw new ArgumentNullException();
			return new LogSeqNumber(stringValue);
		}
	}
}
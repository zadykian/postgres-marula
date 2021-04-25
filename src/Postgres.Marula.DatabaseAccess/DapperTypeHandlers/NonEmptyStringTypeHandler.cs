using System;
using System.Data;
using Dapper;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.DatabaseAccess.DapperTypeHandlers
{
	/// <summary>
	/// <see cref="NonEmptyString"/> dapper type handler.
	/// </summary>
	internal class NonEmptyStringTypeHandler : SqlMapper.TypeHandler<NonEmptyString>
	{
		/// <inheritdoc />
		public override void SetValue(IDbDataParameter parameter, NonEmptyString value)
		{
			parameter.DbType = DbType.String;
			parameter.Value = value.ToString();
		}

		/// <inheritdoc />
		public override NonEmptyString Parse(object value)
			=> value.ToString() ?? throw new ArgumentNullException();
	}
}
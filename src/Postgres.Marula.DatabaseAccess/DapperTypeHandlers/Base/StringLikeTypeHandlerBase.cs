using System.Data;
using Dapper;

namespace Postgres.Marula.DatabaseAccess.DapperTypeHandlers.Base
{
	/// <summary>
	/// Base handler for types which can be represented as string at database level. 
	/// </summary>
	internal abstract class StringLikeTypeHandlerBase<T> : SqlMapper.TypeHandler<T>
		where T : notnull
	{
		/// <inheritdoc />
		public sealed override void SetValue(IDbDataParameter parameter, T value)
		{
			parameter.DbType = DbType.String;
			parameter.Value = value.ToString();
		}
	}
}
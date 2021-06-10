using System;
using Postgres.Marula.DatabaseAccess.DapperTypeHandlers.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.DatabaseAccess.DapperTypeHandlers
{
	/// <summary>
	/// <see cref="DatabaseObjectName"/> dapper type handler.
	/// </summary>
	internal class DatabaseObjectNameTypeHandler : StringLikeTypeHandlerBase<DatabaseObjectName>
	{
		/// <inheritdoc />
		public override DatabaseObjectName Parse(object value) => value.ToString() ?? throw new ArgumentNullException();
	}
}
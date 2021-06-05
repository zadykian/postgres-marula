using System;
using Postgres.Marula.DatabaseAccess.DapperTypeHandlers.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.DatabaseAccess.DapperTypeHandlers
{
	/// <summary>
	/// <see cref="LogSeqNumber"/> dapper type handler.
	/// </summary>
	internal class LogSeqNumberTypeHandler : StringLikeTypeHandlerBase<LogSeqNumber>
	{
		/// <inheritdoc />
		public override LogSeqNumber Parse(object value) => new(value.ToString() ?? throw new ArgumentNullException());
	}
}
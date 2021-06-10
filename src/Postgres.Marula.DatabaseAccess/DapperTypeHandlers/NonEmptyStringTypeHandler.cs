using System;
using Postgres.Marula.DatabaseAccess.DapperTypeHandlers.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.DatabaseAccess.DapperTypeHandlers
{
	/// <summary>
	/// <see cref="NonEmptyString"/> dapper type handler.
	/// </summary>
	internal class NonEmptyStringTypeHandler : StringLikeTypeHandlerBase<NonEmptyString>
	{
		/// <inheritdoc />
		public override NonEmptyString Parse(object value) => value.ToString() ?? throw new ArgumentNullException();
	}
}
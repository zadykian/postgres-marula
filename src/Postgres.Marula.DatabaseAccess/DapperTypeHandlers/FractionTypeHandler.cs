using System.Data;
using Dapper;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.DatabaseAccess.DapperTypeHandlers
{
	/// <summary>
	/// <see cref="Fraction"/> dapper type handler.
	/// </summary>
	internal class FractionTypeHandler : SqlMapper.TypeHandler<Fraction>
	{
		/// <inheritdoc />
		public override void SetValue(IDbDataParameter parameter, Fraction fraction)
		{
			parameter.DbType = DbType.VarNumeric;
			parameter.Value = (decimal) fraction;
		}

		/// <inheritdoc />
		public override Fraction Parse(object value) => (decimal) value;
	}
}
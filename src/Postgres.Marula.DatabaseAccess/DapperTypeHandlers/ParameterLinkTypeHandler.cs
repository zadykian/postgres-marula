using System.Data;
using Dapper;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Infrastructure.Extensions;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.DatabaseAccess.DapperTypeHandlers
{
	/// <summary>
	/// <see cref="IParameterLink"/> dapper type handler.
	/// </summary>
	internal class ParameterLinkTypeHandler : SqlMapper.TypeHandler<IParameterLink>
	{
		/// <inheritdoc />
		public override void SetValue(IDbDataParameter parameter, IParameterLink value)
		{
			parameter.DbType = DbType.String;
			parameter.Value = value.Name;
		}

		/// <inheritdoc />
		public override IParameterLink Parse(object value)
			=> value
				.ToString()
				.ThrowIfNull()
				.To(parameterName => new ParameterLink(parameterName));
	}
}
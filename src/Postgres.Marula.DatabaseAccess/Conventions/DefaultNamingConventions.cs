using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.DatabaseAccess.Conventions
{
	/// <inheritdoc />
	internal class DefaultNamingConventions : INamingConventions
	{
		/// <inheritdoc />
		public virtual DatabaseObjectName SystemSchemaName => "marula_tool";

		/// <inheritdoc />
		DatabaseObjectName INamingConventions.ValuesHistoryTableName => "parameters_values_history";
	}
}
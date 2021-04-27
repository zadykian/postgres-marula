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

		/// <inheritdoc />
		DatabaseObjectName INamingConventions.ParametersTableName => "calculated_parameters";

		/// <inheritdoc />
		DatabaseObjectName INamingConventions.CalculationStatusEnumName => "calculation_status";

		/// <inheritdoc />
		DatabaseObjectName INamingConventions.ParameterUnitEnumName => "parameter_unit";

		/// <inheritdoc />
		DatabaseObjectName INamingConventions.WalLsnHistoryTableName => "wal_lsn_history";
	}
}
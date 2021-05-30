using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.ParameterProperties.StringRepresentation;
using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.DatabaseAccess.ConnectionFactory;
using Postgres.Marula.DatabaseAccess.Conventions;
using Postgres.Marula.DatabaseAccess.ServerInteraction.Base;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.DatabaseAccess.ServerInteraction
{
	/// <inheritdoc cref="ISystemStorage" />
	internal class DefaultSystemStorage : DatabaseInteractionComponent, ISystemStorage
	{
		private readonly INamingConventions namingConventions;

		public DefaultSystemStorage(
			IDbConnectionFactory dbConnectionFactory,
			INamingConventions namingConventions) : base(dbConnectionFactory)
			=> this.namingConventions = namingConventions;

		/// <inheritdoc />
		async Task ISystemStorage.SaveParameterValuesAsync(IReadOnlyCollection<ParameterValueWithStatus> parameterValues)
		{
			if (parameterValues.Count == 0)
			{
				return;
			}

			var commandText = GetCommandTextToInsertValues(parameterValues);
			var dbConnection = await Connection();
			await dbConnection.ExecuteAsync(commandText);
		}

		/// <summary>
		/// Get insert statement to save calculated values to <see cref="INamingConventions.ValuesHistoryTableName"/> table.
		/// </summary>
		private string GetCommandTextToInsertValues(IEnumerable<ParameterValueWithStatus> parameterValues)
			=> $@"
				with parameter_values (parameter_name, calculated_value, unit, status) as
				(
					select *
					from (
						values
							{parameterValues.Select(ToValuesString).JoinBy($",{Environment.NewLine}")}
					) as values
				)
				insert into {namingConventions.SystemSchemaName}.{namingConventions.ValuesHistoryTableName}
					(parameter_id, calculated_value, unit, status)
				select
					parameters.id,
					parameter_values.calculated_value,
					parameter_values.unit,
					parameter_values.status
				from
					{namingConventions.SystemSchemaName}.{namingConventions.ParametersTableName} as parameters

				-- perform right join to fail in case when
				-- parameters dictionary table is not consistent.
				right join
					parameter_values on parameters.name = parameter_values.parameter_name;";

		/// <summary>
		/// Represent <paramref name="parameterValue"/> as
		/// string like '([param_name], [param_value], [unit], [calculation_status])'.
		/// </summary>
		private NonEmptyString ToValuesString(ParameterValueWithStatus parameterValue)
			=> new []
				{
					$"'{parameterValue.Value.ParameterLink.Name}'",
					$"'{parameterValue.Value.AsString()}'",

					$"'{parameterValue.Value.Unit.StringRepresentation()}'" +
						$"::{namingConventions.SystemSchemaName}.{namingConventions.ParameterUnitEnumName}",

					$"'{parameterValue.CalculationStatus.StringRepresentation()}'" +
						$"::{namingConventions.SystemSchemaName}.{namingConventions.CalculationStatusEnumName}"
				}
				.JoinBy(", ")
				.To(values => $"({values})");

		/// <inheritdoc />
		async Task ISystemStorage.SaveLogSeqNumberAsync(LogSeqNumber logSeqNumber)
		{
			var commandText = $@"
				insert into {namingConventions.SystemSchemaName}.{namingConventions.WalLsnHistoryTableName}
					(wal_insert_location)
				values
					(@{nameof(logSeqNumber)}::pg_catalog.pg_lsn);";

			var dbConnection = await Connection();
			await dbConnection.ExecuteAsync(commandText, new {logSeqNumber});
		}

		/// <inheritdoc />
		IAsyncEnumerable<LogSeqNumber> ISystemStorage.GetLogSeqNumbers(PositiveTimeSpan window)
		{
			var commandText = $@"
				select wal_insert_location
				from {namingConventions.SystemSchemaName}.{namingConventions.WalLsnHistoryTableName}
				where log_timestamp >= (current_timespan - )";

			throw new NotImplementedException();
		}
	}
}
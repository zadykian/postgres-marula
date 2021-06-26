using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.ParameterProperties;
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
		private readonly INamingConventions conventions;

		public DefaultSystemStorage(
			IDbConnectionFactory dbConnectionFactory,
			INamingConventions conventions) : base(dbConnectionFactory)
			=> this.conventions = conventions;

		/// <inheritdoc />
		async Task ISystemStorage.SaveParameterValuesAsync(IReadOnlyCollection<ParameterValueWithStatus> parameterValues)
		{
			if (parameterValues.Count == 0)
			{
				return;
			}

			var commandText = CommandTextToInsertValues(parameterValues);
			var connection = await Connection();
			await connection.ExecuteAsync(commandText);
		}

		/// <summary>
		/// Get insert statement to save calculated values to <see cref="INamingConventions.ValuesHistoryTableName"/> table.
		/// </summary>
		private string CommandTextToInsertValues(IEnumerable<ParameterValueWithStatus> parameterValues)
			=> $@"
				with parameter_values (parameter_name, calculated_value, unit, status) as
				(
					select *
					from (
						values
							{parameterValues.Select(ToValuesString).JoinBy($",{Environment.NewLine}")}
					) as values
				)
				insert into {conventions.SystemSchemaName}.{conventions.ValuesHistoryTableName}
					(parameter_id, calculated_value, unit, status)
				select
					parameters.id,
					parameter_values.calculated_value,
					parameter_values.unit,
					parameter_values.status
				from
					{conventions.SystemSchemaName}.{conventions.ParametersTableName} as parameters

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
					$"'{parameterValue.Value.Link.Name}'",

					$"'{parameterValue.Value}'",

					$"'{parameterValue.Value.Unit.AsString()}'" +
					$"::{conventions.SystemSchemaName}.{conventions.ParameterUnitEnumName}",

					$"'{parameterValue.CalculationStatus.StringRepresentation()}'" +
					$"::{conventions.SystemSchemaName}.{conventions.CalculationStatusEnumName}"
				}
				.JoinBy(", ")
				.To(values => $"({values})");

		/// <inheritdoc />
		async Task ISystemStorage.SaveLogSeqNumberAsync(LogSeqNumber logSeqNumber)
		{
			var commandText = string.Intern($@"
				insert into {conventions.SystemSchemaName}.{conventions.WalLsnHistoryTableName}
					(wal_insert_location)
				values
					(@{nameof(logSeqNumber)}::pg_catalog.pg_lsn);");

			var connection = await Connection();
			await connection.ExecuteAsync(commandText, new {logSeqNumber});
		}

		/// <inheritdoc />
		async IAsyncEnumerable<LsnHistoryEntry> ISystemStorage.GetLsnHistoryAsync(PositiveTimeSpan window)
		{
			var queryText = string.Intern($@"
				select
					log_timestamp       as {nameof(LsnHistoryEntry.LogTimestamp)},
					wal_insert_location as {nameof(LsnHistoryEntry.WalInsertLocation)}
				from {conventions.SystemSchemaName}.{conventions.WalLsnHistoryTableName}
				where log_timestamp >= (current_timestamp - @Window)
				order by log_timestamp;");

			var connection = await Connection();
			var historyEntries = await connection.QueryAsync<LsnHistoryEntry>(queryText, new {Window = (TimeSpan) window});
			foreach (var lsnHistoryEntry in historyEntries) yield return lsnHistoryEntry;
		}

		/// <inheritdoc />
		async Task ISystemStorage.SaveBloatFractionAsync(Fraction averageBloatFraction)
		{
			var commandText = $@"
				insert into {conventions.SystemSchemaName}.{conventions.BloatFractionHistoryTableName}
					(average_bloat_fraction)
				values
					(@{nameof(averageBloatFraction)});";

			var connection = await Connection();
			await connection.ExecuteAsync(commandText, new {averageBloatFraction});
		}

		/// <inheritdoc />
		async IAsyncEnumerable<BloatFractionHistoryEntry> ISystemStorage.GetBloatFractionHistory(PositiveTimeSpan window)
		{
			var queryText = string.Intern($@"
				select
					log_timestamp          as {nameof(BloatFractionHistoryEntry.LogTimestamp)},
					average_bloat_fraction as {nameof(BloatFractionHistoryEntry.AverageBloatFraction)}
				from {conventions.SystemSchemaName}.{conventions.BloatFractionHistoryTableName}
				where log_timestamp >= (current_timestamp - @Window)
				order by log_timestamp;");

			var connection = await Connection();
			var historyEntries = await connection.QueryAsync<BloatFractionHistoryEntry>(queryText, new {Window = (TimeSpan) window});
			foreach (var bloatFractionHistoryEntry in historyEntries) yield return bloatFractionHistoryEntry;
		}
	}
}
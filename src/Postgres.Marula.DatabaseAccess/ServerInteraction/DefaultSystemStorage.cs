using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.Parameters.Properties;
using Postgres.Marula.Calculations.Parameters.Values.Base;
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
			IPreparedDbConnectionFactory dbConnectionFactory,
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
			var dbConnection = await GetConnectionAsync();
			await dbConnection.ExecuteAsync(commandText);
		}

		/// <summary>
		/// Get insert statement to save calculated values to <see cref="INamingConventions.ValuesHistoryTableName"/> table. 
		/// </summary>
		private string GetCommandTextToInsertValues(IEnumerable<ParameterValueWithStatus> parameterValues)
			=> $@"
				with parameter_values (parameter_name, calculated_value, status) as
				(
					select *
					from (
						values
							{parameterValues.Select(ToValuesString).JoinBy($",{Environment.NewLine}")}
					) as values
				)
				insert into {namingConventions.SystemSchemaName}.{namingConventions.ValuesHistoryTableName}
					(parameter_id, calculated_value, status)
				select
					parameters.id,
					parameter_values.calculated_value,
					parameter_values.status
				from
					{namingConventions.SystemSchemaName}.{namingConventions.ParametersTableName} as parameters
				inner join
					parameter_values on parameters.name = parameter_values.parameter_name;";

		/// <summary>
		/// Represent <paramref name="parameterValue"/> as string '(param_name, param_value, calculation_status)'.
		/// </summary>
		private static NonEmptyString ToValuesString(ParameterValueWithStatus parameterValue)
			=> new []
				{
					parameterValue.Value.ParameterLink.Name,
					parameterValue.Value.AsString(),
					GetDatabaseRepresentation(parameterValue.CalculationStatus)
				}
				.Select(value => $"'{value}'")
				.JoinBy(", ")
				.To(values => $"({values})");

		/// <summary>
		/// Get database representation of <paramref name="calculationStatus"/> value.
		/// </summary>
		private static NonEmptyString GetDatabaseRepresentation(CalculationStatus calculationStatus)
			=> calculationStatus switch
			{
				CalculationStatus.Applied =>                        "applied",
				CalculationStatus.RequiresConfirmation =>           "requires_confirmation",
				CalculationStatus.RequiresServerRestart =>          "requires_server_restart",
				CalculationStatus.RequiresConfirmationAndRestart => "requires_confirmation_and_restart",
				_ => throw new ArgumentOutOfRangeException(nameof(calculationStatus), calculationStatus, message: null)
			};
	}
}
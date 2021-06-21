using System.Collections.Generic;
using Dapper;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.PublicApi;
using Postgres.Marula.DatabaseAccess.ConnectionFactory;
using Postgres.Marula.DatabaseAccess.Conventions;
using Postgres.Marula.DatabaseAccess.ServerInteraction.Base;
using Postgres.Marula.DatabaseAccess.ServerInteraction.ViewFactory;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.DatabaseAccess.ServerInteraction
{
	/// <inheritdoc cref="IParameterValues" />
	internal class DefaultParameterValues : DatabaseInteractionComponent, IParameterValues
	{
		private readonly INamingConventions conventions;
		private readonly IValueViewFactory valueViewFactory;

		public DefaultParameterValues(
			INamingConventions conventions,
			IValueViewFactory valueViewFactory,
			IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
		{
			this.conventions = conventions;
			this.valueViewFactory = valueViewFactory;
		}

		/// <inheritdoc />
		async IAsyncEnumerable<IValueView> IParameterValues.MostRecentAsync()
		{
			var queryText = string.Intern($@"
            	with ranked_values as
            	(
            		select
            			parameter_id,
            			calculated_value,
            			row_number() over (
            				partition by parameter_id
            				order by calculation_timestamp desc) as rank
            		from {conventions.SystemSchemaName}.{conventions.ValuesHistoryTableName} as history
            	)
            	select
            		parameters.name                as {nameof(IValueView.Link)},
            		ranked_values.calculated_value as {nameof(IValueView.Value)}
            	from ranked_values
            	inner join {conventions.SystemSchemaName}.{conventions.ParametersTableName} as parameters 
            		on ranked_values.parameter_id = parameters.id
            	where ranked_values.rank = 1;");

			var connection = await Connection();
			var parameterValues = await connection.QueryAsync<(IParameterLink Link, NonEmptyString Value)>(queryText);
			foreach (var (link, value) in parameterValues) yield return await valueViewFactory.CreateAsync(link, value);
		}
	}
}
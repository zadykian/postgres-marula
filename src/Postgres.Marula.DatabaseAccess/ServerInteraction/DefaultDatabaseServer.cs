using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.Parameters.Values;
using Postgres.Marula.DatabaseAccess.ConnectionFactory;
using Postgres.Marula.DatabaseAccess.ServerInteraction.Base;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.DatabaseAccess.ServerInteraction
{
	/// <inheritdoc cref="IDatabaseServer" />
	internal class DefaultDatabaseServer : DatabaseInteractionComponent, IDatabaseServer
	{
		public DefaultDatabaseServer(IPreparedDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
		{
		}

		/// <inheritdoc />
		async Task IDatabaseServer.ApplyToConfigurationAsync(IReadOnlyCollection<IParameterValue> parameterValues)
		{
			if (parameterValues.Count == 0)
			{
				return;
			}

			var commandText = parameterValues
				.Select(value => $"alter system set {value.ParameterLink.Name} = '{value.AsStringValue()}';")
				.Add("select pg_reload_conf();")
				.JoinBy(Environment.NewLine);

			var dbConnection = await GetConnectionAsync();
			await dbConnection.ExecuteAsync(commandText);
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dapper;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.Parameters.Values;
using Postgres.Marula.Calculations.Parameters.Values.Base;
using Postgres.Marula.DatabaseAccess.ConnectionFactory;
using Postgres.Marula.DatabaseAccess.ServerInteraction.Base;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

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

		/// <inheritdoc />
		async Task<IParameterValue> IDatabaseServer.GetParameterValueAsync(NonEmptyString parameterName)
		{
			var dbConnection = await GetConnectionAsync();
			var parameterValueAsString = await dbConnection.QuerySingleAsync<string>($"show {parameterName};");
			var parameterLink = new ParameterLink(parameterName);

			return parameterValueAsString switch
			{
				{ } when Regex.IsMatch(parameterValueAsString, "") => new TimeSpanParameterValue(parameterLink, UInt64.MaxValue),
				_ => throw new ApplicationException($"Failed to parse value '{parameterValueAsString}' of parameter '{parameterName}'.")
			};
		}
	}
}
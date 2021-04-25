using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dapper;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.ParameterProperties;
using Postgres.Marula.Calculations.ParameterProperties.StringRepresentation;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParameterValues;
using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.Calculations.ParameterValues.Raw;
using Postgres.Marula.DatabaseAccess.Configuration;
using Postgres.Marula.DatabaseAccess.ConnectionFactory;
using Postgres.Marula.DatabaseAccess.ServerInteraction.Base;
using Postgres.Marula.DatabaseAccess.ServerInteraction.Exceptions;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

// Database server endpoint - IP address and TCP port.
using Endpoint = System.ValueTuple<System.Net.IPAddress, ushort>;

namespace Postgres.Marula.DatabaseAccess.ServerInteraction
{
	/// <inheritdoc cref="IDatabaseServer" />
	internal class DefaultDatabaseServer : DatabaseInteractionComponent, IDatabaseServer
	{
		private readonly IDatabaseAccessConfiguration configuration;

		public DefaultDatabaseServer(
			IDbConnectionFactory dbConnectionFactory,
			IDatabaseAccessConfiguration configuration) : base(dbConnectionFactory)
			=> this.configuration = configuration;

		/// <inheritdoc />
		async Task IDatabaseServer.ApplyToConfigurationAsync(IReadOnlyCollection<IParameterValue> parameterValues)
		{
			if (parameterValues.Count == 0)
			{
				return;
			}

			var alterSystemCommands = await parameterValues
				.SelectAsync(async value =>
					$"alter system set {value.ParameterLink.Name} = " +
					$"'{await GetValueStringRepresentation(value)}';");

			var commandText = alterSystemCommands
				.Add("select pg_reload_conf();")
				.JoinBy(Environment.NewLine);

			var dbConnection = await Connection();
			var signalWasSentSuccessfully = await dbConnection.QuerySingleAsync<bool>(commandText);

			if (!signalWasSentSuccessfully)
			{
				throw new DatabaseServerConfigurationException("Failed to reload server configuration.");
			}
		}

		/// <summary>
		/// Get parameter value full string representation.
		/// </summary>
		private async ValueTask<NonEmptyString> GetValueStringRepresentation(IParameterValue parameterValue)
		{
			if (parameterValue is not FractionParameterValue fractionParameterValue)
			{
				return $"{parameterValue.AsString()}{parameterValue.Unit.NumberSuffix()}";
			}

			var commandText = string.Intern($@"
				select min_val, max_val
				from pg_catalog.pg_settings
				where name = @{nameof(IParameterLink.Name)};");

			var dbConnection = await Connection();
			var (minValue, maxValue) = await dbConnection.QuerySingleAsync<(decimal, decimal)>(
				commandText,
				new {parameterValue.ParameterLink.Name});

			var multiplier = (minValue, maxValue) switch
			{
				(decimal.Zero, decimal.One) => decimal.One,
				(decimal.Zero, 100)         => 100,
				_ => throw new NotSupportedException(
					$"Fraction parameter range [{minValue} .. {maxValue}] is not " +
					$"supported (parameter '{parameterValue.ParameterLink.Name}').")
			};

			return (fractionParameterValue.Value * multiplier).ToString(CultureInfo.InvariantCulture);
		}

		/// <inheritdoc />
		async Task<RawParameterValue> IDatabaseServer.GetRawParameterValueAsync(NonEmptyString parameterName)
		{
			var commandText = string.Intern($@"
				select current_setting(name), min_val, max_val
				from pg_catalog.pg_settings
				where name = @{nameof(parameterName)};");

			var dbConnection = await Connection();

			var (value, minValue, maxValue) = await dbConnection.QuerySingleAsync<(NonEmptyString, decimal?, decimal?)>(
				commandText,
				new {parameterName});

			return minValue.HasValue && maxValue.HasValue
				? new RawRangeParameterValue(value, (minValue.Value, maxValue.Value))
				: new RawParameterValue(value);
		}

		/// <summary>
		/// Cache of parameter context values.
		/// </summary>
		private static readonly ConcurrentDictionary<NonEmptyString, ParameterContext> contextCache = new();

		/// <inheritdoc />
		async ValueTask<ParameterContext> IDatabaseServer.GetParameterContextAsync(NonEmptyString parameterName)
		{
			if (contextCache.TryGetValue(parameterName, out var parameterContext))
			{
				return parameterContext;
			}

			var commandText = string.Intern($@"
				select context
				from pg_catalog.pg_settings
				where name = @{nameof(parameterName)};");

			var dbConnection = await Connection();
			var stringRepresentation = await dbConnection.QuerySingleAsync<NonEmptyString>(commandText, new {parameterName});

			parameterContext = stringRepresentation.ByStringRepresentation<ParameterContext>();
			contextCache[parameterName] = parameterContext;
			return parameterContext;
		}

		/// <inheritdoc />
		async Task<LogSeqNumber> IDatabaseServer.GetCurrentLogSeqNumberAsync()
		{
			var dbConnection = await Connection();
			return await dbConnection.QuerySingleAsync<LogSeqNumber>("select pg_catalog.pg_current_wal_insert_lsn();");
		}

		/// <summary>
		/// Cache of database server versions.
		/// </summary>
		/// <remarks>
		/// Keys are server endpoints - IP address and TCP port.
		/// </remarks>
		private static readonly ConcurrentDictionary<Endpoint, Version> versionCache = new();

		/// <inheritdoc />
		async ValueTask<Version> IDatabaseServer.GetPostgresVersionAsync()
		{
			var endpoint = GetCurrentEndpoint();

			if (versionCache.TryGetValue(endpoint, out var version))
			{
				return version;
			}

			var dbConnection = await Connection();

			version = (await dbConnection.QuerySingleAsync<string>("show server_version;"))
				.Split()
				.First()
				.To(Version.Parse);

			versionCache[endpoint] = version;
			return version;
		}

		/// <summary>
		/// Get current database server endpoint. 
		/// </summary>
		private Endpoint GetCurrentEndpoint()
		{
			var connectionString = configuration.ConnectionString();
			var host = connectionString["server"].To(str => IPAddress.Parse(str));
			var port = connectionString["port"].To(str => ushort.Parse(str));
			return (host, port);
		}
	}
}
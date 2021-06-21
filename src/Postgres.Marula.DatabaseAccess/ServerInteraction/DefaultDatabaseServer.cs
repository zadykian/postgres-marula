using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dapper;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.ParameterProperties;
using Postgres.Marula.Calculations.ParameterProperties.StringRepresentation;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.Calculations.ParameterValues.Raw;
using Postgres.Marula.Calculations.PublicApi;
using Postgres.Marula.DatabaseAccess.Configuration;
using Postgres.Marula.DatabaseAccess.ConnectionFactory;
using Postgres.Marula.DatabaseAccess.ServerInteraction.Base;
using Postgres.Marula.DatabaseAccess.ServerInteraction.Exceptions;
using Postgres.Marula.DatabaseAccess.ServerInteraction.ViewFactory;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable BuiltInTypeReferenceStyle
using TuplesCount = System.UInt32;

namespace Postgres.Marula.DatabaseAccess.ServerInteraction
{
	/// <inheritdoc cref="IDatabaseServer" />
	internal class DefaultDatabaseServer : DatabaseInteractionComponent, IDatabaseServer
	{
		private readonly IDatabaseAccessConfiguration configuration;
		private readonly IValueViewFactory valueViewFactory;

		public DefaultDatabaseServer(
			IDbConnectionFactory dbConnectionFactory,
			IValueViewFactory valueViewFactory,
			IDatabaseAccessConfiguration configuration) : base(dbConnectionFactory)
		{
			this.configuration = configuration;
			this.valueViewFactory = valueViewFactory;
		}

		/// <inheritdoc />
		async Task IDatabaseServer.ApplyToConfigurationAsync(IReadOnlyCollection<IParameterValue> parameterValues)
		{
			if (parameterValues.Count == 0)
			{
				return;
			}

			var alterSystemCommands = await parameterValues
				.ToAsyncEnumerable()
				.SelectAwait(async value => await valueViewFactory.CreateAsync(value.Link, value.ToString()!))
				.Select(view => view.AsAlterSystem())
				.ToArrayAsync();

			var commandText = alterSystemCommands
				.Add("select pg_reload_conf();")
				.JoinBy(Environment.NewLine);

			var connection = await Connection();
			var signalWasSentSuccessfully = await connection.QuerySingleAsync<bool>(commandText);

			if (!signalWasSentSuccessfully)
			{
				throw new DatabaseServerConfigurationException("Failed to reload server configuration.");
			}
		}

		/// <inheritdoc />
		async Task<RawParameterValue> IDatabaseServer.GetRawParameterValueAsync(IParameterLink parameterLink)
		{
			var commandText = string.Intern($@"
				select
					current_setting(name),
					vartype,
					min_val,
					max_val
				from pg_catalog.pg_settings
				where name = @{nameof(IParameterLink.Name)};");

			var connection = await Connection();

			var (value, typeString, minValue, maxValue) = await connection
				.QuerySingleAsync<(NonEmptyString, NonEmptyString, decimal?, decimal?)>(
					commandText,
					new {parameterLink.Name});

			var rawType = typeString.ByStringRepresentation<RawValueType>();

			return minValue.HasValue && maxValue.HasValue
				? new RawRangeParameterValue(value, rawType, (minValue.Value, maxValue.Value))
				: new RawParameterValue(value, rawType);
		}

		/// <summary>
		/// Cache of parameter context values.
		/// </summary>
		private static readonly ConcurrentDictionary<IParameterLink, ParameterContext> contextCache = new();

		/// <inheritdoc />
		async ValueTask<ParameterContext> IDatabaseServer.GetParameterContextAsync(IParameterLink parameterLink)
		{
			if (contextCache.TryGetValue(parameterLink, out var parameterContext))
			{
				return parameterContext;
			}

			var commandText = string.Intern($@"
				select context
				from pg_catalog.pg_settings
				where name = @{nameof(IParameterLink.Name)};");

			var connection = await Connection();
			var stringRepresentation = await connection.QuerySingleAsync<NonEmptyString>(commandText, new {parameterLink.Name});

			parameterContext = stringRepresentation.ByStringRepresentation<ParameterContext>();
			contextCache[parameterLink] = parameterContext;
			return parameterContext;
		}

		/// <inheritdoc />
		async Task<LogSeqNumber> IDatabaseServer.GetCurrentLogSeqNumberAsync()
		{
			var connection = await Connection();
			return await connection.QuerySingleAsync<LogSeqNumber>("select pg_catalog.pg_current_wal_insert_lsn();");
		}

		/// <summary>
		/// Cache of database server versions.
		/// </summary>
		/// <remarks>
		/// Keys are server endpoints - IP address and TCP port.
		/// </remarks>
		private static readonly ConcurrentDictionary<IPEndPoint, Version> versionCache = new();

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
		/// Localhost <see cref="IPAddress"/> object.
		/// </summary>
		private static readonly IPAddress localhost = new(new byte[] {127, 0, 0, 1});

		/// <summary>
		/// Get current database server endpoint.
		/// </summary>
		private IPEndPoint GetCurrentEndpoint()
		{
			var connectionString = configuration.ConnectionString();
			var host = connectionString["server"].To(str => str == "localhost" ? localhost : IPAddress.Parse(str));
			var port = connectionString["port"].To(str => ushort.Parse(str));
			return new(host, port);
		}

		/// <inheritdoc />
		async Task<TuplesCount> IDatabaseServer.GetAverageTableSizeAsync()
		{
			var queryText = string.Intern(@"
				select avg(n_live_tup + n_dead_tup)
				from pg_catalog.pg_stat_all_tables
				where n_live_tup + n_dead_tup != 0;");

			var connection = await Connection();
			return await connection.ExecuteScalarAsync<TuplesCount>(queryText);
		}

		/// <inheritdoc />
		async Task<Fraction> IDatabaseServer.GetAverageBloatFractionAsync()
		{
			var queryText = string.Intern(@"
				select avg(n_dead_tup / (n_dead_tup + n_live_tup))
				from pg_catalog.pg_stat_all_tables
				where n_live_tup + n_dead_tup != 0;");

			var connection = await Connection();
			return await connection.ExecuteScalarAsync<Fraction>(queryText);
		}

		/// <inheritdoc />
		async IAsyncEnumerable<ParentToChild> IDatabaseServer.GetAllHierarchicalLinks()
		{
			var queryText = string.Intern($@"
				select
					concat_ws('.',
						parent_class.relnamespace::regnamespace,
						parent_class.relname) as {nameof(ParentToChild.Parent)},
					concat_ws('.',
						child_class.relnamespace::regnamespace,
						child_class.relname)  as {nameof(ParentToChild.Child)}
				from pg_catalog.pg_inherits
				inner join pg_catalog.pg_class parent_class
					on pg_inherits.inhparent = parent_class.oid
				inner join pg_catalog.pg_class child_class
					on pg_inherits.inhrelid = child_class.oid
				where parent_class.relkind in ('p', 'r');");

			var connection = await Connection();
			var parentToChildLinks = await connection.QueryAsync<ParentToChild>(queryText);
			foreach (var parentToChild in parentToChildLinks) yield return parentToChild;
		}
	}
}
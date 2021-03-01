using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.ParameterProperties;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParameterValues;
using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.Calculations.ParameterValues.Raw;
using Postgres.Marula.DatabaseAccess.ConnectionFactory;
using Postgres.Marula.DatabaseAccess.ServerInteraction.Base;
using Postgres.Marula.DatabaseAccess.ServerInteraction.Exceptions;
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

			var alterSystemCommands = await parameterValues
				.SelectAsync(async value =>
					$"alter system set {value.ParameterLink.Name} = " +
					$"'{await GetValueStringRepresentation(value)}';");

			var commandText = alterSystemCommands
				.Add("select pg_reload_conf();")
				.JoinBy(Environment.NewLine);

			var dbConnection = await GetConnectionAsync();
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
				return $"{parameterValue.AsString()}{parameterValue.Unit.AsString()}";
			}

			var commandText = string.Intern($@"
				select min_val, max_val
				from pg_catalog.pg_settings
				where name = @{nameof(IParameterLink.Name)};");

			var dbConnection = await GetConnectionAsync();
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

			var dbConnection = await GetConnectionAsync();

			var (parameterValue, minValue, maxValue) = await dbConnection.QuerySingleAsync<(NonEmptyString, decimal?, decimal?)>(
				commandText,
				new {parameterName});

			return minValue.HasValue && maxValue.HasValue
				? new Range<decimal>(minValue.Value, maxValue.Value)
					.To(range => new RawRangeParameterValue(parameterValue, range))
				: new RawParameterValue(parameterValue);
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

			var dbConnection = await GetConnectionAsync();
			var stringRepresentation = await dbConnection.QuerySingleAsync<NonEmptyString>(commandText, new {parameterName});

			parameterContext = GetFromStringRepresentation(stringRepresentation);
			contextCache[parameterName] = parameterContext;
			return parameterContext;
		}

		/// <summary>
		/// Get parameter context from its string representation. 
		/// </summary>
		private static ParameterContext GetFromStringRepresentation(NonEmptyString stringRepresentation)
			=> typeof(ParameterContext)
				.GetFields(BindingFlags.Public | BindingFlags.Static)
				.Select(memberInfo => (
					ContextValue: (ParameterContext) memberInfo.GetValue(obj: null)!,
					StringRepresentation: memberInfo.GetCustomAttribute<StringRepresentationAttribute>()!.Value))
				.Single(tuple => tuple.StringRepresentation == stringRepresentation)
				.ContextValue;
	}
}
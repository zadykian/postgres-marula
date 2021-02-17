using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dapper;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.ParameterProperties;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParameterValues;
using Postgres.Marula.Calculations.ParameterValues.Base;
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
		async Task<IParameterValue> IDatabaseServer.GetParameterValueAsync(NonEmptyString parameterName)
		{
			var (parameterValueAsString, minValue, maxValue) = await GetParameterInfoFromServer(parameterName);

			var parameterLink = new ParameterLink(parameterName);

			return parameterValueAsString switch
			{
				{ } when Regex.IsMatch(parameterValueAsString, "^[0-9]+(ms|s|min|h)$")
					=> ParseTimeSpan(parameterValueAsString)
						.To(timeSpan => new TimeSpanParameterValue(parameterLink, timeSpan)),

				{ } when Regex.IsMatch(parameterValueAsString, "^[0-9]+(B|kB|MB|GB)$")
					=> ParseMemory(parameterValueAsString)
						.To(memory => new MemoryParameterValue(parameterLink, memory)),

				{ } when decimal.TryParse(parameterValueAsString, out var decimalValue)
				         && (minValue, maxValue) == (decimal.Zero, decimal.One)
					=> new FractionParameterValue(parameterLink, decimalValue),

				{ } when decimal.TryParse(parameterValueAsString, out var decimalValue)
				         && (minValue, maxValue) == (decimal.Zero, 100)
					=> decimal
						.Divide(decimalValue, 100)
						.To(fraction => new FractionParameterValue(parameterLink, fraction)),

				{ } when parameterValueAsString == "on"
					=> new BooleanParameterValue(parameterLink, value: true),

				{ } when parameterValueAsString == "off"
					=> new BooleanParameterValue(parameterLink, value: false),

				_ => throw new DatabaseServerConfigurationException(
					$"Failed to parse value '{parameterValueAsString}' of parameter '{parameterName}'.")
			};
		}

		/// <summary>
		/// Get parameter value as string and range of valid values. 
		/// </summary>
		private async Task<(NonEmptyString Value, decimal? MinValue, decimal? MaxValue)> GetParameterInfoFromServer(
			NonEmptyString parameterName)
		{
			var commandText = string.Intern($@"
				select current_setting(name), min_val, max_val
				from pg_catalog.pg_settings
				where name = @{nameof(parameterName)};");

			var dbConnection = await GetConnectionAsync();

			return await dbConnection.QuerySingleAsync<(NonEmptyString, decimal?, decimal?)>(
				commandText,
				new {parameterName});
		}

		/// <summary>
		/// Convert string <paramref name="stringToParse"/> to timespan value.
		/// </summary>
		private static PositiveTimeSpan ParseTimeSpan(string stringToParse)
		{
			var (totalMilliseconds, unit) = ParseToTokens(stringToParse);

			var multiplier = unit switch
			{
				"ms"  => 1,
				"s"   => 1000,
				"min" => 60 * 1000,
				"h"   => 60 * 60 * 1000,
				_     => throw new ArgumentOutOfRangeException(nameof(stringToParse), stringToParse, message: null)
			};

			return TimeSpan.FromMilliseconds(totalMilliseconds * (ulong) multiplier);
		}

		/// <summary>
		/// Convert string <paramref name="stringToParse"/> to memory value.
		/// </summary>
		private static Memory ParseMemory(string stringToParse)
		{
			var (totalBytes, unit) = ParseToTokens(stringToParse);

			var multiplier = unit switch
			{
				"B"  => 1,
				"kB" => 1024,
				"MB" => 1024 * 1024,
				"GB" => 1024 * 1024 * 1024,
				_    => throw new ArgumentOutOfRangeException(nameof(stringToParse), stringToParse, message: null)
			};

			return new Memory(totalBytes * (ulong) multiplier);
		}

		/// <summary>
		/// Parse string <paramref name="stringToParse"/> to number and unit tokens.
		/// </summary>
		private static (ulong Value, string Unit) ParseToTokens(string stringToParse)
		{
			var value = stringToParse
				.TakeWhile(char.IsDigit)
				.ToArray()
				.To(charArray => new string(charArray))
				.To(ulong.Parse);

			var unit = stringToParse
				.SkipWhile(char.IsDigit)
				.ToArray()
				.To(charArray => new string(charArray));

			return (Value: value, Unit: unit);
		}

		/// <inheritdoc />
		async Task<ParameterContext> IDatabaseServer.GetParameterContextAsync(NonEmptyString parameterName)
		{
			var commandText = string.Intern($@"
				select context
				from pg_catalog.pg_settings
				where name = @{nameof(parameterName)};");

			var dbConnection = await GetConnectionAsync();
			var parameterContext = await dbConnection.QuerySingleAsync<NonEmptyString>(commandText, new {parameterName});

			return typeof(ParameterContext)
				.GetFields(BindingFlags.Public | BindingFlags.Static)
				.Select(memberInfo => (
					ContextValue: (ParameterContext) memberInfo.GetValue(obj: null)!,
					StringRepresentation: memberInfo.GetCustomAttribute<StringRepresentationAttribute>()!.Value))
				.Single(tuple => tuple.StringRepresentation == parameterContext)
				.ContextValue;
		}
	}
}
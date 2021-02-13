using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dapper;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.Parameters.Properties;
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
				.Select(value => $"alter system set {value.ParameterLink.Name} = '{value.AsString()}{value.Unit.AsString()}';")
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
				{ } when Regex.IsMatch(parameterValueAsString, "^[0-9]+(ms|s|min|h)$")
					=> ParseTimeSpan(parameterValueAsString)
						.To(timeSpan => new TimeSpanParameterValue(parameterLink, timeSpan)),

				{ } when Regex.IsMatch(parameterValueAsString, "^[0-9]+(B|kB|MB|GB)$")
					=> ParseMemory(parameterValueAsString)
						.To(memory => new MemoryParameterValue(parameterLink, memory)),

				_ => throw new ApplicationException(
					$"Failed to parse value '{parameterValueAsString}' of parameter '{parameterName}'.")
			};
		}

		/// <summary>
		/// Convert string <paramref name="stringToParse"/> to timespan value.
		/// </summary>
		private static PositiveTimeSpan ParseTimeSpan(string stringToParse)
		{
			var (number, unit) = ParseToTokens(stringToParse);

			var multiplier = unit switch
			{
				"ms"  => 1,
				"s"   => 1000,
				"min" => 60 * 1000,
				"h"   => 60 * 60 * 1000,
				_     => throw new ArgumentOutOfRangeException(paramName: null, nameof(unit))
			};

			return TimeSpan.FromMilliseconds(number * (ulong) multiplier);
		}

		/// <summary>
		/// Convert string <paramref name="stringToParse"/> to memory value.
		/// </summary>
		private static Memory ParseMemory(string stringToParse)
		{
			var (number, unit) = ParseToTokens(stringToParse);

			var multiplier = unit switch
			{
				"B"  => 1,
				"kB" => 1024,
				"MB" => 1024 * 1024,
				"GB" => 1024 * 1024 * 1024,
				_    => throw new ArgumentOutOfRangeException(paramName: null, nameof(unit))
			};

			return new Memory(number * (ulong) multiplier);
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
	}
}
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Postgres.Marula.Calculations.Configuration;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.ParameterProperties;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.Calculations.ParameterValues.Parsing;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.Calculations.ParametersManagement
{
	/// <inheritdoc />
	internal class PgSettings : IPgSettings
	{
		private readonly IDatabaseServer databaseServer;
		private readonly IParameterValueParser parameterValueParser;
		private readonly ICalculationsConfiguration configuration;

		private readonly ConcurrentDictionary<IParameterLink, CacheEntry> valuesCache;

		public PgSettings(
			IDatabaseServer databaseServer,
			IParameterValueParser parameterValueParser,
			ICalculationsConfiguration configuration)
		{
			this.databaseServer = databaseServer;
			this.parameterValueParser = parameterValueParser;
			this.configuration = configuration;
			valuesCache = new();
		}

		/// <inheritdoc />
		void IPgSettings.Apply(IParameterValue parameterValue)
		{
			if (parameterValue is NullValue) return;
			valuesCache[parameterValue.ParameterLink] = new(parameterValue, Updated: true);
		}

		/// <inheritdoc />
		IAsyncEnumerable<ParameterValueWithStatus> IPgSettings.AllAppliedAsync()
			=> valuesCache
				.Values
				.Where(entry => entry.Updated)
				.ToAsyncEnumerable()
				.SelectAwait(async entry => new ParameterValueWithStatus(entry.Value, await GetCalculationStatus(entry.Value)));

		/// <summary>
		/// Get database parameter calculation status. 
		/// </summary>
		private async ValueTask<CalculationStatus> GetCalculationStatus(IParameterValue parameterValue)
		{
			var adjustmentIsAllowed = configuration.General().AutoAdjustmentIsEnabled();
			var parameterContext = await databaseServer.GetParameterContextAsync(parameterValue.ParameterLink);

			return (adjustmentIsAllowed, parameterContext.RestartIsRequired()) switch
			{
				( false, false ) => CalculationStatus.RequiresConfirmation,
				( false, true  ) => CalculationStatus.RequiresConfirmationAndRestart,
				( true,  false ) => CalculationStatus.Applied,
				( true,  true  ) => CalculationStatus.RequiresServerRestart
			};
		}

		/// <inheritdoc />
		async Task<TValue> IPgSettings.ReadAsync<TParameter, TValue>()
		{
			var parameterLink = new ParameterLink(typeof(TParameter));

			if (valuesCache.TryGetValue(parameterLink, out var cacheEntry))
			{
				return ((ParameterValueBase<TValue>) cacheEntry.Value).Value;
			}

			var rawParameterValue = await databaseServer.GetRawParameterValueAsync(parameterLink);
			var parameterValue = parameterValueParser.Parse(parameterLink, rawParameterValue);

			if (parameterValue is not ParameterValueBase<TValue> correctTypeValue)
			{
				throw new ArgumentException($"Invalid type argument for parameter '{parameterLink.Name}'.", nameof(TValue));
			}

			valuesCache[parameterLink] = new CacheEntry(correctTypeValue, Updated: false);
			return correctTypeValue.Value;
		}

		/// <inheritdoc />
		async Task IPgSettings.FlushAsync()
		{
			if (!configuration.General().AutoAdjustmentIsEnabled())
			{
				return;
			}

			await valuesCache
				.Values
				.Where(entry => entry.Updated)
				.Select(entry => entry.Value)
				.ToImmutableArray()
				.To(updatedValues => databaseServer.ApplyToConfigurationAsync(updatedValues));
		}

		/// <summary>
		/// Parameter values cache entry.
		/// </summary>
		/// <param name="Value">
		/// Parameter value.
		/// </param>
		/// <param name="Updated">
		/// Value was updated by application and needs to be flushed to database server.
		/// </param>
		private sealed record CacheEntry(IParameterValue Value, bool Updated);
	}
}
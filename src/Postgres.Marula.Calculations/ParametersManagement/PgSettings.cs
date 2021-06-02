using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Postgres.Marula.Calculations.ExternalDependencies;
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
		private readonly ConcurrentDictionary<IParameterLink, CacheEntry> valuesCache;

		public PgSettings(IDatabaseServer databaseServer, IParameterValueParser parameterValueParser)
		{
			this.databaseServer = databaseServer;
			this.parameterValueParser = parameterValueParser;
			valuesCache = new();
		}

		/// <inheritdoc />
		void IPgSettings.Apply(IParameterValue parameterValue)
			=> valuesCache[parameterValue.ParameterLink] = new(parameterValue, Updated: true);

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
		Task IPgSettings.FlushAsync()
			=> valuesCache
				.Values
				.Where(entry => entry.Updated)
				.Select(entry => entry.Value)
				.ToImmutableArray()
				.To(updatedValues => databaseServer.ApplyToConfigurationAsync(updatedValues));

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
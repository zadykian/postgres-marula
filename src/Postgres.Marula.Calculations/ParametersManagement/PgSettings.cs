using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Postgres.Marula.Calculations.ExternalDependencies;
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

		public PgSettings(IDatabaseServer databaseServer, IParameterValueParser parameterValueParser)
		{
			this.databaseServer = databaseServer;
			this.parameterValueParser = parameterValueParser;
		}

		/// <inheritdoc />
		async Task IPgSettings.ApplyAsync(IEnumerable<IParameterValue> parameterValues)
			=> await parameterValues
				.ToImmutableArray()
				.To(values => databaseServer.ApplyToConfigurationAsync(values));

		/// <inheritdoc />
		async Task<TValue> IPgSettings.ReadAsync<TParameter, TValue>()
		{
			var parameterName = typeof(TParameter).Name.ToSnakeCase();
			var rawParameterValue = await databaseServer.GetRawParameterValueAsync(parameterName);
			var parameterValue = parameterValueParser.Parse(parameterName, rawParameterValue);
			return parameterValue is ParameterValueBase<TValue> value
				? value.Value
				: throw new ArgumentException($"Invalid type argument for parameter '{parameterName}'.", nameof(TValue));
		}
	}
}
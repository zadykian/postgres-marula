using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PipelineNet.Middleware;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParametersManagement;
using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.Calculations.Pipeline.MiddlewareComponents
{
	/// <summary>
	/// Pipeline component which is responsible
	/// for parameter values retrieving.
	/// </summary>
	internal class ValueCalculationsMiddleware : IAsyncMiddleware<ParametersManagementContext>
	{
		private readonly IReadOnlyCollection<IParameter> parameters;
		private readonly IPgSettings pgSettings;
		private readonly ILogger<ValueCalculationsMiddleware> logger;

		public ValueCalculationsMiddleware(
			IEnumerable<IParameter> parameters,
			IPgSettings pgSettings,
			ILogger<ValueCalculationsMiddleware> logger)
		{
			this.parameters = parameters.ToImmutableArray();
			this.pgSettings = pgSettings;
			this.logger = logger;
		}

		/// <inheritdoc />
		async Task IAsyncMiddleware<ParametersManagementContext>.Run(
			ParametersManagementContext context,
			Func<ParametersManagementContext, Task> next)
		{
			foreach (var parameter in parameters) await CalculateWithDependencies(parameter);
			await next(context);
		}

		/// <summary>
		/// Calculate parameter <paramref name="parameterToCalculate"/> with all its' dependencies. 
		/// </summary>
		private async ValueTask CalculateWithDependencies(IParameter parameterToCalculate)
		{
			var dependenciesValues = await DependenciesValues(parameterToCalculate).ToArrayAsync();
			dependenciesValues.ForEach(pgSettings.Apply);

			var notCalculated = dependenciesValues
				.OfType<NullValue>()
				.Cast<IParameterValue>()
				.ToImmutableArray();

			if (notCalculated.Any())
			{
				var parameterNames = notCalculated.Select(value => value.ParameterLink.Name);
				logger.LogWarning(
					$"Unable to calculate value of parameter '{parameterToCalculate.Name}' " +
					$"because it has dependencies which are not calculated: [{parameterNames.JoinBy(", ")}].");
				return;
			}

			var parameterValue = await parameterToCalculate.CalculateAsync();
			pgSettings.Apply(parameterValue);
		}

		/// <summary>
		/// Calculate values of all parameters which are required
		/// to calculate value of <paramref name="parameterToCalculate"/>. 
		/// </summary>
		private IAsyncEnumerable<IParameterValue> DependenciesValues(IParameter parameterToCalculate)
			=> parameterToCalculate
				.Dependencies()
				.All()
				.ToAsyncEnumerable()
				.SelectAwait(async parameterType => await parameters
					.Single(parameter => parameter.GetType() == parameterType)
					.CalculateAsync());
	}
}
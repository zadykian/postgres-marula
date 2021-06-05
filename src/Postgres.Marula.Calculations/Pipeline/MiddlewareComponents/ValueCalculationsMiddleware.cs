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
			await parameters
				.ToAsyncEnumerable()
				.SelectAwait(CalculateWithDependencies)
				.ForEachAsync(pgSettings.Apply);

			await next(context);
		}

		/// <summary>
		/// Calculate parameter <paramref name="parameterToCalculate"/> with all its' dependencies. 
		/// </summary>
		private async ValueTask<IParameterValue> CalculateWithDependencies(IParameter parameterToCalculate)
		{
			var dependenciesValues = await DependenciesValues(parameterToCalculate).ToArrayAsync();

			var notCalculated = dependenciesValues
				.OfType<NullValue>()
				.Cast<IParameterValue>()
				.ToImmutableArray();

			if (!notCalculated.Any())
			{
				dependenciesValues.ForEach(pgSettings.Apply);
				return await parameterToCalculate.CalculateAsync();
			}

			var parameterNames = notCalculated.Select(value => value.ParameterLink.Name);
			logger.LogWarning(
				$"Unable to calculate value of parameter '{parameterToCalculate.Name}' " +
				$"because it has dependencies which are not calculated: [{parameterNames.JoinBy(", ")}].");
			return NullValue.Instance;
		}

		/// <summary>
		/// Recursively calculate values of all parameters which are required
		/// to calculate value of <paramref name="parameterToCalculate"/>. 
		/// </summary>
		private IAsyncEnumerable<IParameterValue> DependenciesValues(IParameter parameterToCalculate)
			=> parameterToCalculate
				.Dependencies()
				.All()
				.ToAsyncEnumerable()
				.SelectAwait(async parameterType => await parameters
					.Single(parameter => parameter.GetType() == parameterType)
					.To(CalculateWithDependencies));
	}
}
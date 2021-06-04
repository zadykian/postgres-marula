using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using PipelineNet.Middleware;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParametersManagement;

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

		public ValueCalculationsMiddleware(
			IEnumerable<IParameter> parameters,
			IPgSettings pgSettings)
		{
			this.parameters = parameters.ToImmutableArray();
			this.pgSettings = pgSettings;
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
			await parameterToCalculate
				.Dependencies()
				.All()
				.ToAsyncEnumerable()
				.ForEachAwaitAsync(async parameterType =>
				{
					var valueOfDependency = await parameters
						.Single(parameter => parameter.GetType() == parameterType)
						.CalculateAsync();

					pgSettings.Apply(valueOfDependency);
				});

			var parameterValue = await parameterToCalculate.CalculateAsync();
			pgSettings.Apply(parameterValue);
		}
	}
}
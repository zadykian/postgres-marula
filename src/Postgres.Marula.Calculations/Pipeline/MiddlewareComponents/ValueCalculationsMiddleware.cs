using System;
using System.Linq;
using System.Threading.Tasks;
using PipelineNet.Middleware;

namespace Postgres.Marula.Calculations.Pipeline.MiddlewareComponents
{
	/// <summary>
	/// Pipeline component which is responsible
	/// for parameter values retrieving.
	/// </summary>
	internal class ValueCalculationsMiddleware : IAsyncMiddleware<ParametersManagementContext>
	{
		/// <inheritdoc />
		async Task IAsyncMiddleware<ParametersManagementContext>.Run(
			ParametersManagementContext context,
			Func<ParametersManagementContext, Task> next)
		{
			var parameterValues = await context
				.Parameters
				.ToAsyncEnumerable()
				.SelectAwait(parameter => parameter.CalculateAsync())
				.ToArrayAsync();

			await next(context with {CalculatedValues = parameterValues});
		}
	}
}
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using PipelineNet.Middleware;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.Calculations.Pipeline.MiddlewareComponents
{
	/// <summary>
	/// Pipeline component which is responsible
	/// for parameter values retrieving.
	/// </summary>
	internal class ValueCalculationsMiddleware : IAsyncMiddleware<ParametersManagementContext>
	{
		/// <inheritdoc />
		Task IAsyncMiddleware<ParametersManagementContext>.Run(
			ParametersManagementContext context,
			Func<ParametersManagementContext, Task> next)
			=> context
				.Parameters
				.Select(parameter => parameter.Calculate())
				.ToImmutableArray()
				.To(parameterValues => context with {CalculatedValues = parameterValues})
				.To(next);
	}
}
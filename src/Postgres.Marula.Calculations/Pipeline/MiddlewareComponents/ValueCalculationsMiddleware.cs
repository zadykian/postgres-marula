using System;
using System.Linq;
using System.Threading.Tasks;
using PipelineNet.Middleware;
using Postgres.Marula.Calculations.ParametersManagement;
using Postgres.Marula.Calculations.ParameterValues.Base;

namespace Postgres.Marula.Calculations.Pipeline.MiddlewareComponents
{
	/// <summary>
	/// Pipeline component which is responsible
	/// for parameter values retrieving.
	/// </summary>
	internal class ValueCalculationsMiddleware : IAsyncMiddleware<ParametersManagementContext>
	{
		private readonly IPgSettings pgSettings;

		public ValueCalculationsMiddleware(IPgSettings pgSettings) => this.pgSettings = pgSettings;

		/// <inheritdoc />
		async Task IAsyncMiddleware<ParametersManagementContext>.Run(
			ParametersManagementContext context,
			Func<ParametersManagementContext, Task> next)
		{
			

			var parameterValues = await context
				.Parameters
				.ToAsyncEnumerable()
				.SelectAwait(parameter => parameter.CalculateAsync())
				.Where(value => value is not NullValue)
				.ToArrayAsync();

			await next(context);
		}
	}
}
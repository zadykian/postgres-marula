using System;
using System.Threading.Tasks;
using PipelineNet.Middleware;
using Postgres.Marula.Calculations.ParametersManagement;

namespace Postgres.Marula.Calculations.Pipeline.MiddlewareComponents
{
	/// <summary>
	/// Pipeline component which is responsible
	/// for database server parameters adjustment.
	/// </summary>
	internal class ParametersAdjustmentMiddleware : IAsyncMiddleware<ParametersManagementContext>
	{
		private readonly IPgSettings pgSettings;

		public ParametersAdjustmentMiddleware(IPgSettings pgSettings) => this.pgSettings = pgSettings;

		/// <inheritdoc />
		async Task IAsyncMiddleware<ParametersManagementContext>.Run(
			ParametersManagementContext context,
			Func<ParametersManagementContext, Task> next)
		{
			await pgSettings.FlushAsync();
			await next(context);
		}
	}
}
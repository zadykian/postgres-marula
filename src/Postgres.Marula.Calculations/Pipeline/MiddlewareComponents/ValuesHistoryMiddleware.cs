using System;
using System.Linq;
using System.Threading.Tasks;
using PipelineNet.Middleware;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.ParametersManagement;

namespace Postgres.Marula.Calculations.Pipeline.MiddlewareComponents
{
	/// <summary>
	/// Pipeline component which is responsible
	/// for values history maintenance.
	/// </summary>
	internal class ValuesHistoryMiddleware : IAsyncMiddleware<ParametersManagementContext>
	{
		private readonly IPgSettings pgSettings;
		private readonly ISystemStorage systemStorage;

		public ValuesHistoryMiddleware(
			IPgSettings pgSettings,
			ISystemStorage systemStorage)
		{
			this.pgSettings = pgSettings;
			this.systemStorage = systemStorage;
		}

		/// <inheritdoc />
		async Task IAsyncMiddleware<ParametersManagementContext>.Run(
			ParametersManagementContext context,
			Func<ParametersManagementContext, Task> next)
		{
			var appliedValues = await pgSettings.AllAppliedAsync().ToArrayAsync();
			await systemStorage.SaveParameterValuesAsync(appliedValues);
			await next(context);
		}
	}
}
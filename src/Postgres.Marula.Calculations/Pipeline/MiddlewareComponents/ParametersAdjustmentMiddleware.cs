using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using PipelineNet.Middleware;
using Postgres.Marula.Calculations.Configuration;
using Postgres.Marula.Calculations.ParametersManagement;
using Postgres.Marula.Calculations.Pipeline.MiddlewareComponents.Base;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.Calculations.Pipeline.MiddlewareComponents
{
	/// <summary>
	/// Pipeline component which is responsible
	/// for database server parameters adjustment.
	/// </summary>
	internal class ParametersAdjustmentMiddleware : ParametersMiddlewareBase, IAsyncMiddleware<ParametersManagementContext>
	{
		private readonly IPgSettings pgSettings;

		public ParametersAdjustmentMiddleware(
			IPgSettings pgSettings,
			ICalculationsConfiguration calculationsConfiguration) : base(calculationsConfiguration)
			=> this.pgSettings = pgSettings;

		/// <inheritdoc />
		async Task IAsyncMiddleware<ParametersManagementContext>.Run(
			ParametersManagementContext context,
			Func<ParametersManagementContext, Task> next)
		{
			await context
				.CalculatedValues
				.Where(ParameterAdjustmentIsAllowed)
				.ToImmutableArray()
				.To(parameterValues => pgSettings.ApplyAsync(parameterValues));

			await next(context);
		}
	}
}
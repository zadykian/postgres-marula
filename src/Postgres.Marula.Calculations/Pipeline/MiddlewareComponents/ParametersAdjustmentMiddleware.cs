using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using PipelineNet.Middleware;
using Postgres.Marula.Calculations.Configuration;
using Postgres.Marula.Calculations.ExternalDependencies;
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
		private readonly IDatabaseServer databaseServer;

		public ParametersAdjustmentMiddleware(
			IDatabaseServer databaseServer,
			ICalculationsConfiguration calculationsConfiguration) : base(calculationsConfiguration)
			=> this.databaseServer = databaseServer;

		/// <inheritdoc />
		async Task IAsyncMiddleware<ParametersManagementContext>.Run(
			ParametersManagementContext context,
			Func<ParametersManagementContext, Task> next)
		{
			await context
				.CalculatedValues
				.Where(ParameterAdjustmentIsAllowed)
				.ToImmutableArray()
				.To(parameterValues => databaseServer.ApplyToConfigurationAsync(parameterValues));

			await next(context);
		}
	}
}
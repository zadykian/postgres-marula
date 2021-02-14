using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using PipelineNet.Middleware;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.Pipeline.MiddlewareComponents.Base;
using Postgres.Marula.Infrastructure.Configuration;
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
			IAppConfiguration appConfiguration) : base(appConfiguration)
			=> this.databaseServer = databaseServer;

		/// <inheritdoc />
		Task IAsyncMiddleware<ParametersManagementContext>.Run(
			ParametersManagementContext context,
			Func<ParametersManagementContext, Task> next)
			=> context
				.CalculatedValues
				.Where(ParameterAdjustmentIsAllowed)
				.ToImmutableArray()
				.To(parameterValues => databaseServer.ApplyToConfigurationAsync(parameterValues))
				.ContinueWith(_ => next(context), TaskContinuationOptions.OnlyOnRanToCompletion);
	}
}
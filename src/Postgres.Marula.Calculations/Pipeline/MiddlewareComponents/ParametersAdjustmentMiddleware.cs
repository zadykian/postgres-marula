using System;
using System.Linq;
using System.Threading.Tasks;
using PipelineNet.Middleware;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.Parameters.Values;
using Postgres.Marula.Infrastructure.Configuration;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.Calculations.Pipeline.MiddlewareComponents
{
	/// <summary>
	/// Pipeline component which is responsible
	/// for database server parameters adjustment.
	/// </summary>
	internal class ParametersAdjustmentMiddleware : IAsyncMiddleware<ParametersManagementContext>
	{
		private readonly IDatabaseServer databaseServer;
		private readonly IAppConfiguration appConfiguration;

		public ParametersAdjustmentMiddleware(
			IDatabaseServer databaseServer,
			IAppConfiguration appConfiguration)
		{
			this.databaseServer = databaseServer;
			this.appConfiguration = appConfiguration;
		}

		/// <inheritdoc />
		Task IAsyncMiddleware<ParametersManagementContext>.Run(
			ParametersManagementContext context,
			Func<ParametersManagementContext, Task> next)
			=> context
				.CalculatedValues
				.Where(ParameterAdjustmentIsAllowed)
				.To(parameterValues => databaseServer.ApplyToConfigurationAsync(parameterValues))
				.ContinueWith(_ => next(context), TaskContinuationOptions.OnlyOnRanToCompletion);

		/// <summary>
		/// Check if parameter value <paramref name="parameterValue"/>
		/// can be applied to database server.
		/// </summary>
		private bool ParameterAdjustmentIsAllowed(IParameterValue parameterValue) => appConfiguration.AutoAdjustIsEnabled();
	}
}
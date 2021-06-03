using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using PipelineNet.Middleware;
using Postgres.Marula.Calculations.Configuration;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.ParameterProperties;
using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.Calculations.Pipeline.MiddlewareComponents.Base;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.Calculations.Pipeline.MiddlewareComponents
{
	/// <summary>
	/// Pipeline component which is responsible
	/// for values history maintenance.
	/// </summary>
	internal class ValuesHistoryMiddleware : ParametersMiddlewareBase, IAsyncMiddleware<ParametersManagementContext>
	{
		private readonly ISystemStorage systemStorage;
		private readonly IDatabaseServer databaseServer;

		public ValuesHistoryMiddleware(
			ISystemStorage systemStorage,
			IDatabaseServer databaseServer,
			ICalculationsConfiguration calculationsConfiguration) : base(calculationsConfiguration)
		{
			this.systemStorage = systemStorage;
			this.databaseServer = databaseServer;
		}

		/// <inheritdoc />
		async Task IAsyncMiddleware<ParametersManagementContext>.Run(
			ParametersManagementContext context,
			Func<ParametersManagementContext, Task> next)
		{
			var parameterValues = await context
				.CalculatedValues
				.SelectAsync(async parameterValue => new ParameterValueWithStatus(
					parameterValue,
					await GetCalculationStatus(parameterValue)));

			await parameterValues
				.ToImmutableArray()
				.To(collection => systemStorage.SaveParameterValuesAsync(collection));

			await next(context);
		}

		/// <summary>
		/// Get database parameter calculation status. 
		/// </summary>
		private async ValueTask<CalculationStatus> GetCalculationStatus(IParameterValue parameterValue)
		{
			var adjustmentIsAllowed = ParameterAdjustmentIsAllowed(parameterValue);
			var parameterContext = await databaseServer.GetParameterContextAsync(parameterValue.ParameterLink);			

			return (adjustmentIsAllowed, parameterContext.RestartIsRequired()) switch
			{
				( false, false ) => CalculationStatus.RequiresConfirmation,
				( false, true  ) => CalculationStatus.RequiresConfirmationAndRestart,
				( true,  false ) => CalculationStatus.Applied,
				( true,  true  ) => CalculationStatus.RequiresServerRestart
			};
		}
	}
}
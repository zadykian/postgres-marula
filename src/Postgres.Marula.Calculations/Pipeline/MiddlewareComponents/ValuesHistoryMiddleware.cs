using System;
using System.Linq;
using System.Threading.Tasks;
using PipelineNet.Middleware;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.ParameterProperties;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.Calculations.Pipeline.MiddlewareComponents.Base;
using Postgres.Marula.Infrastructure.Configuration;
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
			IAppConfiguration appConfiguration) : base(appConfiguration)
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
				.Join(
					context.Parameters,
					value => value.ParameterLink,
					parameter => parameter.GetLink(),
					(value, parameter) => (Parameter: parameter, Value: value))
				.SelectAsync(async tuple => new ParameterValueWithStatus(
					tuple.Value,
					await GetCalculationStatus(tuple.Parameter, tuple.Value)));

			await parameterValues
				.ToReadOnlyCollection()
				.To(systemStorage.SaveParameterValuesAsync);

			await next(context);
		}

		/// <summary>
		/// Get database parameter calculation status. 
		/// </summary>
		private async Task<CalculationStatus> GetCalculationStatus(IParameterLink parameterLink, IParameterValue parameterValue)
		{
			var adjustmentIsAllowed = ParameterAdjustmentIsAllowed(parameterValue);
			var parameterContext = await databaseServer.GetParameterContextAsync(parameterLink.Name);			

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
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using PipelineNet.Middleware;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.Parameters.Properties;
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

		public ValuesHistoryMiddleware(
			ISystemStorage systemStorage,
			IAppConfiguration appConfiguration) : base(appConfiguration)
			=> this.systemStorage = systemStorage;

		/// <inheritdoc />
		Task IAsyncMiddleware<ParametersManagementContext>.Run(
			ParametersManagementContext context,
			Func<ParametersManagementContext, Task> next)
			=> context
				.CalculatedValues
				.Join(
					context.Parameters,
					value => value.ParameterLink,
					parameter => parameter.GetLink(),
					(value, parameter) => new ParameterValueWithStatus(value, GetCalculationStatus(value, parameter)))
				.ToImmutableArray()
				.To(parameterValues => systemStorage.SaveParameterValuesAsync(parameterValues))
				.ContinueWith(_ => next(context), TaskContinuationOptions.OnlyOnRanToCompletion);

		/// <summary>
		/// Get database parameter calculation status. 
		/// </summary>
		private CalculationStatus GetCalculationStatus(IParameterValue parameterValue, IParameter parameter)
			=> (ParameterAdjustmentIsAllowed(parameterValue), parameter.Context.RestartIsRequired()) switch
			{
				( false, false ) => CalculationStatus.RequiresConfirmation,
				( false, true  ) => CalculationStatus.RequiresConfirmationAndRestart,
				( true,  false ) => CalculationStatus.Applied,
				( true,  true  ) => CalculationStatus.RequiresServerRestart
			};
	}
}
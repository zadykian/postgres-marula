using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Postgres.Marula.Calculations.Configuration;
using Postgres.Marula.Calculations.Jobs;
using Postgres.Marula.Calculations.Jobs.Base;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParameterValueParsing;
using Postgres.Marula.Calculations.Pipeline;
using Postgres.Marula.Calculations.Pipeline.Factory;
using Postgres.Marula.Calculations.Pipeline.MiddlewareComponents;
using Postgres.Marula.Infrastructure.AppComponents;
using Postgres.Marula.Infrastructure.Extensions;

[assembly: InternalsVisibleTo("Postgres.Marula.Tests")]

namespace Postgres.Marula.Calculations
{
	/// <inheritdoc />
	public class CalculationsAppComponent : IAppComponent
	{
		/// <inheritdoc />
		void IAppComponent.RegisterServices(IServiceCollection serviceCollection)
			=> serviceCollection
				.AddSingleton<ICalculationsConfiguration, DefaultCalculationsConfiguration>()
				.AddBasedOn<IParameter>(ServiceLifetime.Scoped)
				.Forward<IParameter, IParameterLink>()
				.AddTransient<ParametersManagementContext>()
				.AddTransient<ValueCalculationsMiddleware>()
				.AddTransient<ParametersAdjustmentMiddleware>()
				.AddTransient<ValuesHistoryMiddleware>()
				.AddSingleton<IPipelineFactory, DefaultPipelineFactory>()
				.AddSingleton<IJob, GeneralCalculationsJob>()
				.AddSingleton<IParameterValueParser, DefaultParameterValueParser>();
	}
}
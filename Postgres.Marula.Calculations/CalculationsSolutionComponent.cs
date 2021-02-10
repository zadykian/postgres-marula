using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Postgres.Marula.Calculations.Jobs;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.Pipeline;
using Postgres.Marula.Calculations.Pipeline.Factory;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.SolutionComponents;

// ReSharper disable UnusedType.Global

[assembly: InternalsVisibleTo("Postgres.Marula.Tests")]

namespace Postgres.Marula.Calculations
{
	/// <inheritdoc />
	internal class CalculationsSolutionComponent : ISolutionComponent
	{
		/// <inheritdoc />
		void ISolutionComponent.RegisterServices(IServiceCollection serviceCollection)
			=> serviceCollection
				.AddBasedOn<IParameter>(ServiceLifetime.Transient)
				.AddTransient<ParametersManagementContext>()
				.AddSingleton<IPipelineFactory, DefaultPipelineFactory>()
				.AddSingleton<ICalculationJob, TimerCalculationJob>();
	}
}
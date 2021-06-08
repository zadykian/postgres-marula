using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Postgres.Marula.Calculations.Configuration;
using Postgres.Marula.Calculations.HardwareInfo;
using Postgres.Marula.Calculations.Jobs.Base;
using Postgres.Marula.Calculations.Parameters.Autovacuum.Bloat;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.Parameters.Wal.LsnHistory;
using Postgres.Marula.Calculations.ParametersManagement;
using Postgres.Marula.Calculations.ParameterValues.Parsing;
using Postgres.Marula.Calculations.Pipeline;
using Postgres.Marula.Calculations.Pipeline.Factory;
using Postgres.Marula.Calculations.Pipeline.MiddlewareComponents;
using Postgres.Marula.HwInfo;
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
				.AddSingleton<ICalculationsConfiguration, CalculationsConfiguration>()
				.AddSingleton<IParameterValueParser, DefaultParameterValueParser>()
				.AddScoped<IPgSettings, PgSettings>()
				.AddScoped<IWalLsnHistory, WalLsnHistory>()
				.AddScoped<IBloatAnalysis, BloatAnalysis>()
				.AddSingleton<IHardwareInfo, RemoteHardwareInfo>()
				.AddBasedOn<IParameter>(ServiceLifetime.Scoped)
				.Forward<IParameter, IParameterLink>()
				.To(RegisterPipelineServices)
				.AddBasedOn<IJob>();

		/// <summary>
		/// Register services related to calculations pipeline.
		/// </summary>
		private static IServiceCollection RegisterPipelineServices(IServiceCollection serviceCollection)
			=> serviceCollection
				.AddTransient<ParametersManagementContext>()
				.AddTransient<ValueCalculationsMiddleware>()
				.AddTransient<ParametersAdjustmentMiddleware>()
				.AddTransient<ValuesHistoryMiddleware>()
				.AddSingleton<IPipelineFactory, DefaultPipelineFactory>();
	}
}
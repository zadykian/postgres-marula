using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Postgres.Marula.Calculations;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Infrastructure.Configuration;
using Postgres.Marula.Infrastructure.TypeDecorators;
using Postgres.Marula.Tests.Base;
using Postgres.Marula.Tests.Calculations.FakeServices;

namespace Postgres.Marula.Tests.Calculations.Base
{
	/// <summary>
	/// Base class for testing services from calculations component.
	/// </summary>
	internal abstract class CalculationsTestFixtureBase : SingleComponentTestFixtureBase<CalculationsSolutionComponent>
	{
		/// <inheritdoc />
		protected override void ConfigureServices(IServiceCollection serviceCollection)
		{
			base.ConfigureServices(serviceCollection);

			serviceCollection
				.AddSingleton<IAppConfiguration, TestAppConfiguration>()
				.AddSingleton<FakeDatabaseServer>()
				.AddSingleton<IDatabaseServer>(provider => provider.GetRequiredService<FakeDatabaseServer>())
				.AddSingleton<IDatabaseServerAccessTracker>(provider => provider.GetRequiredService<FakeDatabaseServer>())

				.AddSingleton<ISystemStorage, FakeSystemStorage>()
				.RemoveAll<IParameter>()
				.AddScoped<IParameter, SharedBuffersFakeParameter>()
				.AddScoped<IParameter, AutovacuumVacuumCostDelayFakeParameter>();
		}

		/// <inheritdoc />
		private sealed class TestAppConfiguration : DefaultAppConfiguration
		{
			public TestAppConfiguration(IConfiguration configuration) : base(configuration)
			{
			}

			/// <inheritdoc />
			public override PositiveTimeSpan GetRecalculationInterval() => TimeSpan.FromMilliseconds(value: 10);
		}
	}
}
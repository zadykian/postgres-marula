using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Postgres.Marula.Calculations;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.Parameters.Base;
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
			=> serviceCollection
				.AddSingleton<IDatabaseServer, FakeDatabaseServer>()
				.AddSingleton<ISystemStorage, FakeSystemStorage>()
				.RemoveAll<IParameter>()
				.AddScoped<IParameter, SharedBuffersFakeParameter>()
				.AddScoped<IParameter, AutovacuumVacuumCostDelayFakeParameter>();
	}
}
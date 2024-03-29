using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Configuration;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.PeriodicJobs.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.Calculations.PeriodicJobs.Implementations
{
	/// <inheritdoc cref="IJob"/>
	/// <remarks>
	/// This implementation is responsible for average bloat fraction logging.
	/// </remarks>
	internal class BloatFractionLoggingJob : JobBase
	{
		public BloatFractionLoggingJob(
			ICalculationsConfiguration configuration,
			IServiceScopeFactory serviceScopeFactory,
			ILogger<BloatFractionLoggingJob> logger) : base(configuration.Autovacuum().Interval(), serviceScopeFactory, logger)
		{
		}

		/// <inheritdoc />
		public override NonEmptyString Name => "bloat fraction logging";

		/// <inheritdoc />
		protected override async ValueTask ExecuteAsync(IServiceScope serviceScope)
		{
			var averageBloatFraction = await serviceScope
				.ServiceProvider
				.GetRequiredService<IDatabaseServer>()
				.GetAverageBloatFractionAsync();

			await serviceScope
				.ServiceProvider
				.GetRequiredService<ISystemStorage>()
				.SaveBloatFractionAsync(averageBloatFraction);
		}
	}
}
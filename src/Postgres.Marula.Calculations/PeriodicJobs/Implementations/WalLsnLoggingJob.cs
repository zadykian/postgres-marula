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
	/// This implementation is responsible for WAL insert location logging.
	/// </remarks>
	internal class WalLsnLoggingJob : JobBase
	{
		public WalLsnLoggingJob(
			ICalculationsConfiguration configuration,
			IServiceScopeFactory serviceScopeFactory,
			ILogger<WalLsnLoggingJob> logger) : base(configuration.Wal().Interval(), serviceScopeFactory, logger)
		{
		}

		/// <inheritdoc />
		protected override NonEmptyString Description => "wal insert location logging";

		/// <inheritdoc />
		protected override async ValueTask ExecuteAsync(IServiceScope serviceScope)
		{
			var currentLogSeqNumber = await serviceScope
				.ServiceProvider
				.GetRequiredService<IDatabaseServer>()
				.GetCurrentLogSeqNumberAsync();

			await serviceScope
				.ServiceProvider
				.GetRequiredService<ISystemStorage>()
				.SaveLogSeqNumberAsync(currentLogSeqNumber);
		}
	}
}
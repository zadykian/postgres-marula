using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Configuration;
using Postgres.Marula.Calculations.Jobs.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.Jobs
{
	/// <inheritdoc cref="IJob"/>
	/// <remarks>
	/// This implementation is responsible for WAL insert location tracking.
	/// </remarks>
	internal class WalLsnTrackingJob : JobBase
	{
		public WalLsnTrackingJob(
			ICalculationsConfiguration configuration,
			IServiceScopeFactory serviceScopeFactory,
			ILogger<JobBase> logger) : base(configuration.LsnTrackingInterval(), serviceScopeFactory, logger)
		{
		}

		/// <inheritdoc />
		protected override NonEmptyString Description => "wal insert location tracking";

		/// <inheritdoc />
		protected override async ValueTask ExecuteAsync(IServiceScope serviceScope)
		{
			// todo
			await ValueTask.CompletedTask;
		}
	}
}
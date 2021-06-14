using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Jobs.Base;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.AppHost
{
	/// <summary>
	/// Service to run and stop all long-running jobs.
	/// </summary>
	internal class JobRunner : BackgroundService
	{
		private readonly IReadOnlyCollection<IJob> jobs;
		private readonly ILogger<JobRunner> logger;

		public JobRunner(
			IEnumerable<IJob> jobs,
			ILogger<JobRunner> logger)
		{
			this.jobs = jobs.ToImmutableArray();
			this.logger = logger;
		}

		/// <inheritdoc />
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			logger.LogInformation("starting all jobs.");
			jobs.ForEach(job => job.Start());
			logger.LogInformation("all jobs are started.");
			await Task.CompletedTask;
		}

		/// <inheritdoc />
		public override async Task StopAsync(CancellationToken cancellationToken)
		{
			logger.LogInformation("stopping all executing jobs.");
			jobs.ForEach(job => job.Stop());
			logger.LogInformation("all jobs are stopped.");
			await base.StopAsync(cancellationToken);
		}
	}
}
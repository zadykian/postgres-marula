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
	/// <inheritdoc />
	internal class HostedService : BackgroundService
	{
		private readonly IReadOnlyCollection<IJob> jobs;
		private readonly ILogger<HostedService> logger;

		public HostedService(
			IEnumerable<IJob> jobs,
			ILogger<HostedService> logger)
		{
			this.jobs = jobs.ToImmutableArray();
			this.logger = logger;
		}

		/// <inheritdoc />
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			jobs.ForEach(job => job.Start());
			logger.LogInformation("Marula application is running.");
			await Task.CompletedTask;
		}

		/// <inheritdoc />
		public override async Task StopAsync(CancellationToken cancellationToken)
		{
			logger.LogInformation("Marula application is stopping.");
			jobs.ForEach(job => job.Stop());
			await base.StopAsync(cancellationToken);
		}
	}
}
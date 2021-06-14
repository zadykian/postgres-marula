using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.PeriodicJobs.Base;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.Calculations.PeriodicJobs.PublicApi
{
	/// <inheritdoc />
	internal class Jobs : IJobs
	{
		private readonly IReadOnlyCollection<IJob> jobs;
		private readonly ILogger<Jobs> logger;

		public Jobs(
			IEnumerable<IJob> jobs,
			ILogger<Jobs> logger)
		{
			this.jobs = jobs.ToImmutableArray();
			this.logger = logger;
		}

		/// <inheritdoc />
		IAsyncEnumerable<IJobInfo> IJobs.InfoAboutAll()
			=> jobs
				.Select(job => new JobInfo(job.Name, job.State))
				.ToAsyncEnumerable();

		/// <inheritdoc />
		async ValueTask IJobs.StartAllAsync()
		{
			await ValueTask.CompletedTask;
			logger.LogInformation("starting all jobs.");
			jobs.ForEach(job => job.Start());
			logger.LogInformation("all jobs are started.");
		}

		/// <inheritdoc />
		async ValueTask IJobs.StopAllAsync()
		{
			await ValueTask.CompletedTask;
			logger.LogInformation("stopping all executing jobs.");
			jobs.ForEach(job => job.Stop());
			logger.LogInformation("all jobs are stopped.");
		}
	}
}
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.PeriodicJobs.Base;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

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
		IReadOnlyCollection<IJobInfo> IJobs.InfoAboutAll()
			=> jobs
				.Select(job => new JobInfo(job.Name, job.State))
				.ToImmutableArray();

		/// <inheritdoc />
		void IJobs.StartAll()
		{
			logger.LogInformation("starting all jobs.");
			jobs.ForEach(job => job.Start());
			logger.LogInformation("all jobs are started.");
		}

		/// <inheritdoc />
		void IJobs.StopAll()
		{
			logger.LogInformation("stopping all executing jobs.");
			jobs.ForEach(job => job.Stop());
			logger.LogInformation("all jobs are stopped.");
		}

		/// <inheritdoc cref="IJobInfo"/>
		private sealed record JobInfo(NonEmptyString Name, JobState State) : IJobInfo;
	}
}
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Postgres.Marula.Calculations.PeriodicJobs.PublicApi;

namespace Postgres.Marula.AppHost
{
	/// <summary>
	/// Service to run and stop all long-running jobs.
	/// </summary>
	internal class JobRunner : BackgroundService
	{
		private readonly IJobs jobs;

		public JobRunner(IJobs jobs) => this.jobs = jobs;

		/// <inheritdoc />
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			jobs.StartAll();
			await Task.CompletedTask;
		}

		/// <inheritdoc />
		public override async Task StopAsync(CancellationToken cancellationToken)
		{
			jobs.StopAll();
			await base.StopAsync(cancellationToken);
		}
	}
}
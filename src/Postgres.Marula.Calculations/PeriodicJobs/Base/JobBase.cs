using System;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.PeriodicJobs.PublicApi;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.PeriodicJobs.Base
{
	/// <inheritdoc cref="IJob"/>
	/// <remarks>
	/// This type provides base functionality for scoped periodic executions.
	/// </remarks>
	internal abstract class JobBase : IJob, IDisposable
	{
		private readonly Timer timer;
		private readonly IServiceScopeFactory serviceScopeFactory;
		private readonly ILogger<JobBase> logger;

		protected JobBase(
			PositiveTimeSpan executionInterval,
			IServiceScopeFactory serviceScopeFactory,
			ILogger<JobBase> logger)
		{
			timer = CreateTimer(executionInterval);
			this.serviceScopeFactory = serviceScopeFactory;
			this.logger = logger;
		}

		/// <inheritdoc />
		public abstract NonEmptyString Name { get; }

		/// <inheritdoc />
		JobState IJobInfo.State => timer.Enabled ? JobState.Running : JobState.Stopped;

		/// <summary>
		/// Perform single iteration in service scope.
		/// </summary>
		protected abstract ValueTask ExecuteAsync(IServiceScope serviceScope);

		/// <summary>
		/// <see cref="Timer.Elapsed"/> event handler.
		/// </summary>
		private async ValueTask OnTimerElapsed()
		{
			logger.LogInformation($"[{Name}] iteration is started.");

			var serviceScope = serviceScopeFactory.CreateScope();
			try
			{
				await ExecuteAsync(serviceScope);
				logger.LogInformation($"[{Name}] iteration is completed.");
			}
			catch (Exception exception)
			{
				logger.LogError(exception, $"[{Name}] iteration is failed.");
			}
			finally
			{
				serviceScope.Dispose();
				timer.Start();
			}
		}

		/// <summary>
		/// Create timer based on <paramref name="executionInterval"/>. 
		/// </summary>
		private Timer CreateTimer(PositiveTimeSpan executionInterval)
			=> executionInterval
				.To(interval => new Timer(interval.TotalMilliseconds) {AutoReset = false})
				.Then(intervalTimer => intervalTimer.Elapsed += async (_, _) => await OnTimerElapsed());

		/// <inheritdoc />
		void IJob.Start()
		{
			timer.Start();
			logger.LogInformation($"[{Name}] job is started.");
		}

		/// <inheritdoc />
		void IJob.Stop()
		{
			timer.Stop();
			logger.LogInformation($"[{Name}] job is stopped.");
		}

		/// <inheritdoc />
		void IDisposable.Dispose() => timer.Dispose();
	}
}
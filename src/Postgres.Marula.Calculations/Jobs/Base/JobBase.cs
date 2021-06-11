using System;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.Jobs.Base
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

		/// <summary>
		/// Job description.
		/// </summary>
		protected abstract NonEmptyString Description { get; }

		/// <summary>
		/// Perform single iteration in service scope.
		/// </summary>
		protected abstract ValueTask ExecuteAsync(IServiceScope serviceScope);

		/// <summary>
		/// <see cref="Timer.Elapsed"/> event handler.
		/// </summary>
		private async ValueTask OnTimerElapsed()
		{
			logger.LogInformation($"[{Description}] iteration is started.");

			var serviceScope = serviceScopeFactory.CreateScope();
			try
			{
				await ExecuteAsync(serviceScope);
			}
			catch (Exception exception)
			{
				logger.LogError(exception, $"[{Description}] occured error during iteration.");
				throw;
			}
			finally
			{
				serviceScope.Dispose();
				timer.Start();
			}

			logger.LogInformation($"[{Description}] iteration completed successfully.");
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
			logger.LogInformation($"[{Description}] job is started.");
		}

		/// <inheritdoc />
		void IJob.Stop()
		{
			timer.Stop();
			logger.LogInformation($"[{Description}] job is stopped.");
		}

		/// <inheritdoc />
		void IDisposable.Dispose() => timer.Dispose();
	}
}
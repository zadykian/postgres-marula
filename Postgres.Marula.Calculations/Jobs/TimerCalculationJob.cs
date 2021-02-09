using System;
using System.Timers;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Infrastructure.Configuration;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.Calculations.Jobs
{
	/// <inheritdoc cref="ICalculationJob"/>
	internal class TimerCalculationJob : ICalculationJob, IDisposable
	{
		private readonly Timer timer;
		private readonly ILogger<TimerCalculationJob> logger;

		public TimerCalculationJob(
			IAppConfiguration appConfiguration,
			ILogger<TimerCalculationJob> logger)
		{
			timer = appConfiguration
				.GetRecalculationInterval()
				.To(interval => new Timer(interval.TotalMilliseconds) {AutoReset = false})
				.Then(intervalTimer => intervalTimer.Elapsed += (_, _) => OnTimerElapsed());

			this.logger = logger;
		}

		/// <inheritdoc />
		void ICalculationJob.Run() => timer.Start();

		/// <inheritdoc />
		void IDisposable.Dispose() => timer.Dispose();

		/// <summary>
		/// <see cref="Timer.Elapsed"/> event handler. 
		/// </summary>
		private void OnTimerElapsed()
		{
			logger.LogInformation("Parameters calculation iteration is started.");

			try
			{
				RunCalculationIteration();
			}
			catch (Exception exception)
			{
				logger.LogError(exception, "Occured error during parameters calculation iteration.");
				throw;
			}
			finally
			{
				timer.Start();
			}

			logger.LogInformation("Parameters calculation iteration completed successfully.");
		}

		/// <summary>
		/// Run new iteration of database parameters calculation.
		/// </summary>
		private void RunCalculationIteration()
		{
			// todo
		}
	}
}
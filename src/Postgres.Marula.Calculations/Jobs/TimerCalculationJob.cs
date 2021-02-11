using System;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Pipeline.Factory;
using Postgres.Marula.Infrastructure.Configuration;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.Calculations.Jobs
{
	/// <inheritdoc cref="ICalculationJob"/>
	internal class TimerCalculationJob : ICalculationJob, IDisposable
	{
		private readonly Timer timer;
		private readonly IPipelineFactory pipelineFactory;
		private readonly IServiceScopeFactory serviceScopeFactory;
		private readonly ILogger<TimerCalculationJob> logger;

		public TimerCalculationJob(
			IAppConfiguration appConfiguration,
			IPipelineFactory pipelineFactory,
			IServiceScopeFactory serviceScopeFactory,
			ILogger<TimerCalculationJob> logger)
		{
			timer = CreateTimer(appConfiguration);
			this.pipelineFactory = pipelineFactory;
			this.serviceScopeFactory = serviceScopeFactory;
			this.logger = logger;
		}

		/// <summary>
		/// Create timer based on <paramref name="appConfiguration"/>. 
		/// </summary>
		private Timer CreateTimer(IAppConfiguration appConfiguration)
			=> appConfiguration
				.GetRecalculationInterval()
				.To(interval => new Timer(interval.TotalMilliseconds) {AutoReset = false})
				.Then(intervalTimer => intervalTimer.Elapsed += async (_, _) => await OnTimerElapsed());

		/// <inheritdoc />
		void ICalculationJob.Run() => timer.Start();

		/// <inheritdoc />
		void IDisposable.Dispose() => timer.Dispose();

		/// <summary>
		/// <see cref="Timer.Elapsed"/> event handler. 
		/// </summary>
		private async Task OnTimerElapsed()
		{
			logger.LogInformation("Parameters calculation iteration is started.");

			var serviceScope = serviceScopeFactory.CreateScope();
			try
			{
				await pipelineFactory
					.CreateWithScope(serviceScope)
					.RunAsync();
			}
			catch (Exception exception)
			{
				logger.LogError(exception, "Occured error during parameters calculation iteration.");
				throw;
			}
			finally
			{
				serviceScope.Dispose();
				timer.Start();
			}

			logger.LogInformation("Parameters calculation iteration completed successfully.");
		}
	}
}
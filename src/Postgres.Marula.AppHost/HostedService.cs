using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Jobs;

namespace Postgres.Marula.AppHost
{
	/// <inheritdoc />
	internal class HostedService : BackgroundService
	{
		private readonly ICalculationJob calculationJob;
		private readonly ILogger<HostedService> logger;

		public HostedService(
			ICalculationJob calculationJob,
			ILogger<HostedService> logger)
		{
			this.calculationJob = calculationJob;
			this.logger = logger;
		}

		/// <inheritdoc />
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			calculationJob.Run();
			logger.LogInformation("Marula application is running.");
			await Task.CompletedTask;
		}
	}
}
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Jobs;

namespace Postgres.Marula.AppHost
{
	/// <inheritdoc />
	internal class MarulaService : BackgroundService
	{
		private readonly ICalculationJob calculationJob;
		private readonly ILogger<MarulaService> logger;

		public MarulaService(
			ICalculationJob calculationJob,
			ILogger<MarulaService> logger)
		{
			this.calculationJob = calculationJob;
			this.logger = logger;
		}

		/// <inheritdoc />
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			calculationJob.Run();
			logger.LogInformation("marula application is running.");
			await Task.CompletedTask;
		}
	}
}
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Postgres.Marula.Service
{
	/// <inheritdoc />
	internal class Worker : BackgroundService
	{
		private readonly ILogger<Worker> logger;

		public Worker(ILogger<Worker> logger) => this.logger = logger;

		/// <inheritdoc />
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
				await Task.Delay(1000, stoppingToken);
			}
		}
	}
}
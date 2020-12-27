using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Postgres.Marula.AppHost
{
	/// <inheritdoc />
	internal class Worker : BackgroundService
	{
		private readonly ILogger<Worker> logger;

		public Worker(ILogger<Worker> logger) => this.logger = logger;

		/// <inheritdoc />
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			logger.LogInformation("app is running");
			await Task.CompletedTask;
		}
	}
}
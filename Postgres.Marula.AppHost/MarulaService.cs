using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Postgres.Marula.AppHost
{
	/// <inheritdoc />
	internal class MarulaService : BackgroundService
	{
		private readonly ILogger<MarulaService> logger;

		public MarulaService(ILogger<MarulaService> logger) => this.logger = logger;

		/// <inheritdoc />
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			logger.LogInformation("app is running");
			await Task.CompletedTask;
		}
	}
}
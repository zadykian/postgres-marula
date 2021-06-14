using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Postgres.Marula.AppControl.UIElements.Lifetime;

namespace Postgres.Marula.AppControl
{
	/// <summary>
	/// Terminal UI service.
	/// </summary>
	internal class TerminalUiService : IHostedService
	{
		private readonly IUIStartup uiStartup;
		private readonly IUIShutdown uiShutdown;

		public TerminalUiService(
			IUIStartup uiStartup,
			IUIShutdown uiShutdown)
		{
			this.uiStartup = uiStartup;
			this.uiShutdown = uiShutdown;
		}

		/// <inheritdoc />
		async Task IHostedService.StartAsync(CancellationToken cancellationToken) => await uiStartup.StartAsync();

		/// <inheritdoc />
		async Task IHostedService.StopAsync(CancellationToken cancellationToken) => await uiShutdown.StopAsync();
	}
}
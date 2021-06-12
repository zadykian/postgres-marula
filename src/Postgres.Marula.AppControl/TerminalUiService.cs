using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Terminal.Gui;

namespace Postgres.Marula.AppControl
{
	/// <summary>
	/// Terminal UI service.
	/// </summary>
	internal class TerminalUiService : IHostedService
	{
		/// <inheritdoc />
		async Task IHostedService.StartAsync(CancellationToken cancellationToken)
		{
			await Task.CompletedTask;
			Application.Run<ControlWindow>();
		}

		/// <inheritdoc />
		async Task IHostedService.StopAsync(CancellationToken cancellationToken)
		{
			await Task.CompletedTask;
			Application.RequestStop();
		}
	}
}
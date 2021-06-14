using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Postgres.Marula.AppControl.UIElements;
using Terminal.Gui;

namespace Postgres.Marula.AppControl
{
	/// <summary>
	/// Terminal UI service.
	/// </summary>
	internal class TerminalUiService : IHostedService
	{
		private readonly ControlWindow controlWindow;

		public TerminalUiService(ControlWindow controlWindow) => this.controlWindow = controlWindow;

		/// <inheritdoc />
		async Task IHostedService.StartAsync(CancellationToken cancellationToken)
		{
			Application.Init();
			await controlWindow.InitializeAsync();
			Application.Run(controlWindow);
		}

		/// <inheritdoc />
		async Task IHostedService.StopAsync(CancellationToken cancellationToken)
		{
			await Task.CompletedTask;
			Application.RequestStop();
		}
	}
}
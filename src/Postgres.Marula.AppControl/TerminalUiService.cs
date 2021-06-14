using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Postgres.Marula.AppControl.UIElements;

namespace Postgres.Marula.AppControl
{
	/// <summary>
	/// Terminal UI service.
	/// </summary>
	internal class TerminalUiService : IHostedService
	{
		private readonly IUserInterface userInterface;

		public TerminalUiService(IUserInterface userInterface) => this.userInterface = userInterface;

		/// <inheritdoc />
		async Task IHostedService.StartAsync(CancellationToken cancellationToken) => await userInterface.StartAsync();

		/// <inheritdoc />
		async Task IHostedService.StopAsync(CancellationToken cancellationToken) => await userInterface.StopAsync();
	}
}